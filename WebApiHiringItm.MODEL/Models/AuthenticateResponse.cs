using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Models
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public Guid? IdRoll { get; set; }
        public string UserEmail { get; set; }
        public bool Permission { get; set; }
        public string accessToken { get; set; }
        public string code { get; set; }


        public AuthenticateResponse(UserT user, string token, string _code)
        {
            Id = user.Id.ToString();
            UserName = user.UserName;
            UserEmail = user.UserEmail;         
            IdRoll = user.RollId;
            accessToken = token;
            code = _code;

        }
        public AuthenticateResponse(Contractor user, string token, string _code)
        {
            Id = user.Id.ToString();
            UserName = user.Nombre ;
            UserEmail = user.Correo;
            accessToken = token;
            code = _code;

        }
    }
}
