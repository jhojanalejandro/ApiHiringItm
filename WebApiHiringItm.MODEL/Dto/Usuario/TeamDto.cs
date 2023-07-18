using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Usuario
{
    public class TeamDto
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string Identification { get; set; }
        public Guid? UserFirmId { get; set; }
        public string? RollCode { get; set; }
        public string? RollId { get; set; }
        public string? Professionalposition { get; set; }

    }
}
