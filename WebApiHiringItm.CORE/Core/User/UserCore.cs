using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.MODEL.Dto;
using System.Security.Principal;
using System.Security.Cryptography;
using MimeKit;
using MailKit.Security;
using System.Net.Mail;
using System.Net;
using MimeKit.Text;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.CORE.Helpers.Enums.Rolls;
using WebApiHiringItm.CORE.Helpers.Enums;
using Microsoft.EntityFrameworkCore;

namespace WebApiHiringItm.Core.User
{
    public class UserCore : IUserCore
    {
        #region VARIABLE
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        private readonly MailSettings _mailSettings;
        static readonly byte[] keys = Encoding.UTF8.GetBytes("401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1");
        private readonly AppSettings _appSettings;
        #endregion
        #region CONTRUCTOR
        public UserCore(HiringContext context, IMapper mapper, IOptions<AppSettings> appSettings, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _mailSettings = mailSettings.Value;
        }
        #endregion

        #region PUBLIC METODS
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            if (model.UserType.Equals(RollEnum.Contratista.Description()))
            {
                var getUserC = _context.Contractor
                    .FirstOrDefault(x => x.Correo.Equals(model.Username) && x.ClaveUsuario.Equals(model.Password));

                if (getUserC == null)
                {
                    return null;
                }
                var map = _mapper.Map<AuthDto>(getUserC);

                var token = generateJwtToken(map);

                return new AuthenticateResponse(getUserC, token, model.UserType);
            }
            else
            {
                var getUser = _context.UserT
                    .Include(r => r.Roll)
                    .FirstOrDefault(x => x.UserEmail.Equals(model.Username) && x.UserPassword.Equals(model.Password) && !x.Roll.Code.Equals(RollEnum.Desactivada.Description()));

                if (getUser == null)
                {
                    return null;
                }
                var map = _mapper.Map<AuthDto>(getUser);

                var token = generateJwtToken(map);

                return new AuthenticateResponse(getUser, token, model.UserType);
            }

        }

        public async Task<List<UserTDto>> GetAll()
        {
            var result = _context.UserT
                .Include(x => x.Roll)
                .Where(x => !x.Roll.Code.Equals(RollEnum.Contratista.Description()));

            return await result.Select(ct => new UserTDto()
            {
                Id = ct.Id,
                UserName = ct.UserName,
                Code = ct.Roll.Code,
                Avatar = ct.Avatar,
                UserEmail = ct.UserEmail,
                Professionalposition = ct.Professionalposition,
                Identification = ct.Identification,
                PhoneNumber  = ct.PhoneNumber,
            })
            .AsNoTracking()
            .ToListAsync();

        }

        public async Task<List<UserTDto>> GetAllAdmins()
        {
            var result = _context.UserT
                .Include(x => x.Roll)
                .Where(x => !x.Roll.Code.Equals(RollEnum.Desactivada.Description()) && !x.Roll.Code.Equals(RollEnum.Contratista.Description()) && (x.Roll.Code.Equals(RollEnum.Supervisor.Description()) || x.Roll.Code.Equals(RollEnum.Admin.Description())));

            return await result.Select(ct => new UserTDto()
            {
                Id = ct.Id,
                UserName = ct.UserName,
                Code = ct.Roll.Code,
                Avatar = ct.Avatar,
                UserEmail = ct.UserEmail,
                Professionalposition = ct.Professionalposition,
                Identification = ct.Identification,
                PhoneNumber = ct.PhoneNumber,
            })
            .AsNoTracking()
            .ToListAsync();

        }

        public UserT GetByIdd(Guid id)
        {
            return _context.UserT.FirstOrDefault(x => x.Id == id);
        }
        public async Task<UserTDto> GetById(Guid id)
        {
            var result = _context.UserT.FirstOrDefault(x => x.Id == id);
            var map = _mapper.Map<UserTDto>(result);
            return await Task.FromResult(map);
        }
        public async Task<bool> GetUserForgetPassword(RetrievePassword model)
        {
            var result = _context.UserT.FirstOrDefault(x => x.UserEmail == model.UserEmail && x.UserName == model.UserName);

            if (result != null)
            {
                var message = new MailRequest();
                message.Body = "hei que tal, hemos visto que perdiste tu clave, aqui te damos la clave que olvidaste, recuerda por seguridad actualizarla cuando inicies sesion" + "Tu Clave es: " + result.UserPassword;
                message.ToEmail = result.UserEmail;
                message.Subject = "rifa familiar Clave perdida";

                if (await sendMessage(message))
                {
                    return true;
                }

            }
            return false;
        }
        public async Task<bool> Update(UserTDto model)
        {
            if (model.Id != null)
            {
                try
                {
                    var rollId = _context.Roll.Where(x => x.Code.Equals(model.Code)).Select(w => w.Id).FirstOrDefault();
                    var userupdate = _context.UserT.FirstOrDefault(x => x.Id.Equals(model.Id));
                    if (userupdate != null)
                    {
                        userupdate.RollId = rollId;
                        model.UserPassword = userupdate.UserPassword;
                        var map = _mapper.Map(model, userupdate);
                        _context.UserT.Update(map);
                        var res = await _context.SaveChangesAsync();
                        return res != 0 ? true : false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error", ex);
                }

            }
            return false;
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
            try
            {
                if (model.Id != null)
                {
                    var userupdate = _context.UserT.FirstOrDefault(x => x.Id == model.Id);
                    var map = _mapper.Map(model, userupdate);
                    _context.UserT.Update(map);
                    var res = await _context.SaveChangesAsync();
                    return res != 0 ? true : false;

                }

            }
            catch (Exception e)
            {

                new Exception("Error", e);
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
        public async Task<string> SignUp(UserTDto model)
        {
            var getRoll = _context.Roll.FirstOrDefault(x => x.Code.Equals(RollEnum.Desactivada.Description()));
            var userupdate = _context.UserT.FirstOrDefault(x => x.UserEmail.Equals(model.UserEmail));
            if (userupdate != null)
            {
                return null;
            }
            else
            {
                var map = _mapper.Map<UserT>(model);
                map.Id = Guid.NewGuid();
                map.RollId = getRoll.Id;
                _context.UserT.Add(map);
                await _context.SaveChangesAsync();
                return map.Id != null ? "Registro Exitoso" : null;
            }
            return null;
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

        //private bool message()
        //{
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    var mailMessage = new MimeMessage();
        //    mailMessage.From.Add(new MailboxAddress("alejoyepes.1000@gmail.com"));
        //    mailMessage.To.Add(new MailboxAddress("alejoyepes18@gmail.com"));
        //    mailMessage.Subject = "SendMail_MailKit_WithDomain";
        //    mailMessage.Body = new TextPart(TextFormat.Plain)
        //    {
        //        Text = "Hello"
        //    };

        //    using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
        //    {
        //        smtpClient.Connect("smtp.gmail.com", 25, SecureSocketOptions.StartTlsWhenAvailable);
        //        smtpClient.Send(mailMessage);
        //        smtpClient.Disconnect(true);
        //    }
        //    return false;
        //}
        private async Task<bool> sendMessage(MailRequest mailRequest)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.From.Add(MailboxAddress.Parse("alejoyepes.1000@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            //CreateTestMessage2();
            //message();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            var resp = smtp;
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            try
            {
                await smtp.SendAsync(email);
                return true;

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }

            return false;

        }
        #endregion
    }
}