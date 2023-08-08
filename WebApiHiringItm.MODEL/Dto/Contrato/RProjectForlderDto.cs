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
        public bool? Activate { get; set; }
        public bool? EnableProject { get; set; }
        public int? ContractorsCant { get; set; }
        public string? NumberProject { get; set; }
        public string? Project { get; set; }
        public string? Rubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? StatusContractId { get; set; }
        public string? ObjectContract { get; set; }

        public DetalleContratoDto DetalleContratoDto { get; set; }

    }
}
