using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class RProjectForlderDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string DescriptionProject { get; set; }
        public bool? Execution { get; set; }
        public bool? Activate { get; set; }
        public bool? EnableProject { get; set; }
        public int? ContractorsCant { get; set; }
        public string NumberProject { get; set; }
        public DetalleContratoDto DetalleContratoDto { get; set; }

    }
}
