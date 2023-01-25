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
        public int Id { get; set; }
        public Guid contractorId { get; set; }
        public Guid? contractId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int IdRoll { get; set; }
        public string UserEmail { get; set; }
        public bool Permission { get; set; }
        public string accessToken { get; set; }

        public AuthenticateResponse(UserT user, string token)
        {
            Id = user.Id;
            UserName = user.UserName;
            UserPassword = user.UserPassword;
            UserEmail = user.UserEmail;         
            IdRoll = user.RollId;
            accessToken = token;
        }
        public AuthenticateResponse(ContractorDto user, string token)
        {
            contractorId = user.Id;
            contractId = user.ContractId;
            UserName = user.Nombre ;
            UserPassword = user.ClaveUsuario;
            UserEmail = user.Correo;
            IdRoll = 6;
            accessToken = token;
        }
    }
}
