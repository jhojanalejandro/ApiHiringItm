using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class RProjectForlderDto
    {
        public Guid? Id { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public bool? EnableProject { get; set; }
        public string? NumberProject { get; set; }
        public string? Project { get; set; }
        public string? ObjectContract { get; set; }
        public string? DutyContract { get; set; }
        public string? AreaCode { get; set; }


        public DetailContractDto DetalleContrato { get; set; }

    }
}
