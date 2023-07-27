using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Usuario
{
    public class UserTDto
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string? Code { get; set; }
        public string? Professionalposition { get; set; }
        public string? UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string Identification { get; set; }
        public Guid? UserFirmId { get; set; }
        public string? PasswordMail { get; set; }
        public Guid? RollId { get; set; }
    }
}
