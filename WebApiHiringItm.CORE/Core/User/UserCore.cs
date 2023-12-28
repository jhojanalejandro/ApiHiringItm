using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Core.Share;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Helpers.Enums;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.Core.User
{
    public class UserCore : IUserCore
    {
        #region VARIABLE
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        static readonly byte[] keys = Encoding.UTF8.GetBytes("401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1");
        private readonly AppSettings _appSettings;
        private readonly IMessageHandlingCore _messageHandling;
        #endregion
        #region CONTRUCTOR
        public UserCore(HiringContext context, IMapper mapper, IOptions<AppSettings> appSettings, IMessageHandlingCore messageHandling)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _messageHandling = messageHandling;
        }
        #endregion

        #region PUBLIC METODS
        public IGenericResponse<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.UserType))
                return ApiResponseHelper.CreateErrorResponse<AuthenticateResponse>(Resource.AUTENTICATEFIELDERROR);

            if (model.UserType.Equals(RollEnum.Contratista.Description()))
            {
                var getUserC = _context.Contractor
                    .FirstOrDefault(x => x.Correo.Equals(model.Username));

                if (getUserC == null)
                {
                    return ApiResponseHelper.CreateErrorResponse<AuthenticateResponse>(Resource.ERRORAUTENTICATE);
                }
                else
                {
                    var password = GenericCore.Descrypt(getUserC.ClaveUsuario);
                    if (!model.Password.Equals(password))
                    {
                        return ApiResponseHelper.CreateErrorResponse<AuthenticateResponse>(Resource.ERRORAUTENTICATE);
                    }

                }
                var map = _mapper.Map<AuthDto>(getUserC);

                var token = generateJwtToken(map);

                var resp = new AuthenticateResponse(getUserC, token, model.UserType);
                return ApiResponseHelper.CreateResponse(resp);

            }
            else
            {
                var getUser = _context.UserT
                    .Include(r => r.Roll)
                    .FirstOrDefault(x => x.UserEmail.Equals(model.Username) && x.UserPassword.Equals(model.Password) && !x.Roll.Code.Equals(RollEnum.Desactivada.Description()));

                if (getUser == null)
                {
                    return ApiResponseHelper.CreateErrorResponse<AuthenticateResponse>(Resource.ERRORAUTENTICATE);
                }
                var map = _mapper.Map<AuthDto>(getUser);

                var token = generateJwtToken(map);

                var resp=  new AuthenticateResponse(getUser, token, getUser.Roll.Code);
                return ApiResponseHelper.CreateResponse(resp);
            }

        }

        public async Task<List<TeamDto>> GetTeam()
        {
            return await _context.UserT
                .Include(x => x.Roll).Select(ct => new TeamDto()
            {
                Id = ct.Id,
                UserName = ct.UserName,
                UserEmail = ct.UserEmail,
                Identification = ct.Identification,
                PhoneNumber  = ct.PhoneNumber,
                RollCode = ct.Roll.Code,
                Professionalposition = ct.Professionalposition,
                RollId = ct.Roll.Id.ToString(),
                RollDescription = ct.Roll.RollName
            })
            .AsNoTracking()
            .ToListAsync();

        }

        public UserT GetByIdd(Guid id)
        {
            return _context.UserT.FirstOrDefault(x => x.Id == id);
        }

        public async Task<UserTDto?> GetById(Guid id)
        {
            var result = _context.UserT.Where(x => x.Id == id);
            return await result.Select(us => new UserTDto
            {
                Id = us.Id,
                UserName = us.UserName, 
                UserEmail = us.UserEmail,
                Code = us.Roll.Code,
                UserPassword = us.UserPassword,
                PhoneNumber = us.PhoneNumber,
                Professionalposition = us.Professionalposition,
                Identification = us.Identification,
                PasswordMail = us.PasswordMail

            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }
        
        public async Task<IGenericResponse<string>> GetUserForgetPassword(ForgotPasswordRequest mail)
        {
            var resultUser = _context.UserT.FirstOrDefault(x => x.UserEmail.Equals(mail.Mail));
            var resultContractors = _context.Contractor.FirstOrDefault(x => x.Correo.Equals(mail.Mail));
            bool activateReset = false;
            bool userC = true;
            var userId = "";
            var userMail = "";
            if (resultUser != null)
            {
                userId = GenericCore.Encrypt(resultUser.Id.ToString());
                userC = false;
                userMail = resultUser.UserEmail;
                resultUser.EnableChangePassword = true;
                _context.UserT.Update(resultUser);
            }
            else if (resultContractors != null)
            {
                userId = GenericCore.Encrypt(resultContractors.Id.ToString());
                userC = false;
                resultContractors.EnableChangePassword = true;
                userMail = resultContractors.Correo;
                _context.Contractor.Update(resultContractors);
            }
            if (string.IsNullOrEmpty(userId))
                return ApiResponseHelper.CreateErrorResponse<string>("El sistema no puede enviar el correo electronico se puede deber");
           

            var resp = await _messageHandling.SendMessageForgotPasswors(userMail,userId);
            if (resp)
            {
                _context.SaveChanges();
                return ApiResponseHelper.CreateResponse<string>(null, true, Resource.RESETPASSWORDSUCCESS);
            }

            return ApiResponseHelper.CreateErrorResponse<string>(Resource.MAILNOTEXIST);

        }

        public async Task<IGenericResponse<string>> UpdateTeamRoll(UserTDto model)
        {
            if (model.Id == null || model.Id == Guid.Empty)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.GUIDNOTVALID);

            var userupdate = _context.UserT.FirstOrDefault(x => x.Id.Equals(model.Id));
            if (userupdate != null)
            {
                if (model.RollId == null || model.RollId == Guid.Empty)
                {
                    model.RollId = userupdate.RollId;
                }
                model.UserPassword = userupdate.UserPassword;
                model.PasswordMail = userupdate.PasswordMail;
                var map = _mapper.Map(model, userupdate);
                _context.UserT.Update(map);
                await _context.SaveChangesAsync();
            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.UPDATESUCCESSFULL);
        }

        public async Task<bool> UpdatePassword(UserUpdatePasswordDto model)
        {
            if (model.Id != null)
            {
                var userupdate = _context.UserT.FirstOrDefault(x => x.Id == model.Id);
                var map = _mapper.Map(model, userupdate);
                _context.UserT.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;

            }
            return false;
        }
        
        public async Task<bool> UpdateRoll(UpdateRollDto model)
        {
            if (model.Id != null)
            {
                var userupdate = _context.UserT.FirstOrDefault(x => x.Id == model.Id);
                var map = _mapper.Map(model, userupdate);
                _context.UserT.Update(map);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;
        }
        
        public async Task<bool> Delete(Guid id)
        {
            var user = _context.UserT.Where(x => x.Id == id).FirstOrDefault();
            if (user != null)
            {

                var result = _context.UserT.Remove(user);
                var res = await _context.SaveChangesAsync();
                return res != 0 ? true : false;
            }
            return false;
        }
        
        public async Task<IGenericResponse<string>> SignUp(UserTDto model)
        {
            var password = GenericCore.Encrypt(model.UserPassword);
            var passwordMail = GenericCore.Encrypt(model.PasswordMail);

            var getRoll = _context.Roll.FirstOrDefault(x => x.Code.Equals(RollEnum.Desactivada.Description()));
            var userupdate = _context.UserT.FirstOrDefault(x => x.UserEmail.Equals(model.UserEmail));
            if (userupdate != null)
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.USEREXIST);
            }
            else
            {
                var map = _mapper.Map<UserT>(model);
                map.Id = Guid.NewGuid();
                map.RollId = getRoll.Id;
                map.UserPassword = password;
                map.PasswordMail = passwordMail;
                _context.UserT.Add(map);
                await _context.SaveChangesAsync();
            }
            return ApiResponseHelper.CreateResponse<string>(null, true, Resource.REGISTERSUCCESSFULL);


        }
        
        public string generateJwtToken(AuthDto user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public async Task<bool> ValidateT(string authToken)
        {
            if (IsJwtTokenValid(authToken))
            {
                return true;
            }
            return false;
        }

        public async Task<IGenericResponse<string>> ResetPasswordUser(ResetPasswordRequest updatePassword)
        {
            var userId = GenericCore.Descrypt(updatePassword.Id.ToString());
            var userupdate = _context.UserT.FirstOrDefault(x => x.Id.Equals(Guid.Parse(userId)) && x.EnableChangePassword == true);
            var contractorupdate = _context.Contractor.FirstOrDefault(x => x.Id.Equals(Guid.Parse(userId)) && x.EnableChangePassword == true);

            if (userupdate != null)
            {
                userupdate.EnableChangePassword = false;
                userupdate.PasswordMail = updatePassword.Password;
                _context.UserT.Update(userupdate);
            }else if (contractorupdate != null)
            {
                contractorupdate.EnableChangePassword = false;
                contractorupdate.ClaveUsuario = updatePassword.Password;
                _context.Contractor.Update(contractorupdate);
            }
            if (userupdate == null)
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.DISABLEMAIL);

            await _context.SaveChangesAsync();
            return ApiResponseHelper.CreateResponse<string>(null,true,Resource.RESETSUCCESS);
        }

        #endregion

        #region PRIVATE METODS

        private static bool IsJwtTokenValid(string token)
        {
            // example of token:
            //                  header                              payload                                      signature
            // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJmb29AZ21haWwuY29tIiwiZXhwIjoxNjQ1NzM1MDU2fQ.Gtrm2G_35ynyNd1-CjZ1HsvvFFItEsXPvwhaOsN81HQ

            // from JWT spec
            static string Base64UrlEncode(byte[] input)
            {
                var output = Convert.ToBase64String(input);
                output = output.Split('=')[0]; // Remove any trailing '='s
                output = output.Replace('+', '-'); // 62nd char of encoding
                output = output.Replace('/', '_'); // 63rd char of encoding
                return output;
            }

            try
            {
                // position of second period in order to split header+payload and signature
                int index = token.IndexOf('.', token.IndexOf('.') + 1);

                // Example: Gtrm2G_35ynyNd1-CjZ1HsvvFFItEsXPvwhaOsN81HQ
                string signature = token[(index + 1)..];

                // Bytes of header + payload
                // In other words bytes of: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJmb29AZ21haWwuY29tIiwiZXhwIjoxNjQ1NzM1MDU2fQ
                byte[] bytesToSign = Encoding.UTF8.GetBytes(token[..index]);

                // compute hash
                var hash = new HMACSHA256(keys).ComputeHash(bytesToSign);
                var computedSignature = Base64UrlEncode(hash);

                // make sure that signatures match
                return computedSignature.Length == signature.Length
                    && computedSignature.SequenceEqual(signature);
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}