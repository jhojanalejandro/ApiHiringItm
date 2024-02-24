using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Usuario
{
    public class UserUpdatePasswordDto
    {
        public Guid Id { get; set; }
        public string UserPassword { get; set; }
        public string PasswordMail { get; set; }

    }
}
