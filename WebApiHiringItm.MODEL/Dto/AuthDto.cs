using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class AuthDto
    {
        public string Id { get; set; }  
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? RollId { get; set; }

    }
}
