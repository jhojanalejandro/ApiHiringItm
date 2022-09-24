using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class UserTDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Professionalposition { get; set; }
        //public string LastName { get; set; }
        public string UserPassword { get; set; }
        public int IdRoll { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        //public string IdentificationCard { get; set; }
    }
}
