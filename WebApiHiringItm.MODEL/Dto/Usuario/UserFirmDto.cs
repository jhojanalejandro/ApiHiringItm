using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Usuario
{
    public class UserFirmDto
    {
        public int Id { get; set; }
        public int? RollId { get; set; }
        public string FirmUser { get; set; }
        public string FirmData { get; set; }
    }
}
