using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Core.Share.Interface;
using WebApiHiringItm.CORE.Helpers;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.CORE.Helpers.GenericResponse.Interface;
using WebApiHiringItm.CORE.Properties;
using WebApiHiringItm.MODEL.Dto.Share;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.CORE.Core.Share
{
    public class GenericCore : IGenericCore
    {
        #region VARIABLE
        private readonly HiringContext _context;
        private readonly IMapper _mapper;
        #endregion
        #region CONTRUCTOR
        public GenericCore(HiringContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        private static readonly string key = "1234567890123456"; // Debes cambiar esto y asegurarte de que ambas partes usen la misma clave.

        #region Public methods
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Puedes personalizar el IV, pero ambos lados deben usar el mismo.

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Descrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // Puedes personalizar el IV, pero ambos lados deben usar el mismo.

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }


        public async Task<IGenericResponse<string>> UpdateSessionPanel(SessionPanelDto sessionPanel)
        {
            try
            {
                var sessionUser = _context.SessionPanel.Where(x => x.PanelCode.Equals(sessionPanel.PanelCode) && x.ContractId.Equals(sessionPanel.ContractId));
                var sessionUserActivate = _context.SessionPanel.Where(x => x.UserId.Equals(sessionPanel.UserId) && x.Activate == true && x.PanelCode.Equals(sessionPanel.PanelCode) && x.ContractId.Equals(sessionPanel.ContractId));

                if (sessionUserActivate != null && sessionPanel.ActivateSession)
                {
                    return ApiResponseHelper.CreateResponse<string>(null, true, "");
                }
                if (sessionPanel.ActivateSession)
                {
                    if (sessionUser != null)
                    {
                        var activateSession = sessionUser.Where(x => x.Activate == true).FirstOrDefault();
                        if (activateSession != null)
                        {
                            return ApiResponseHelper.CreateErrorResponse<string>(Resource.INTERACTIVECOMPONENT);
                        }
                        var validateSessContract = sessionUser.Where(x => x.Activate == true && x.ContractorId.Equals(sessionPanel.ContractorId)).FirstOrDefault();
                        if (validateSessContract != null)
                        {
                            return ApiResponseHelper.CreateErrorResponse<string>(Resource.INTERACTIVECOMPONENT);
                        }

                    }
                    var map = _mapper.Map<SessionPanel>(sessionPanel);
                    _context.SessionPanel.Add(map);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    sessionUser = _context.SessionPanel.Where(x => x.UserId.Equals(sessionPanel.UserId) && x.Activate == true);
                    if (sessionUser != null)
                    {
                        var session = sessionUser.FirstOrDefault();
                        session.Activate = false;
                        session.FinalSessionDate = DateTime.Now;
                        _context.SessionPanel.Update(session);

                    }
                }

                return ApiResponseHelper.CreateResponse<string>(null, true,"");

            }
            catch (Exception ex)
            {
                return ApiResponseHelper.CreateErrorResponse<string>(Resource.DOCUMENTGENERATE);

            }

        }
        #endregion

    }
}
