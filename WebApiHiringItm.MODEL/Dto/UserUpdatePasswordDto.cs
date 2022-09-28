using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class UserUpdatePasswordDto
    {
        public int Id { get; set; }
        public string UserPassword { get; set; }
    }
}
