using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contrato
{
    public class ContractListDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public string? ObjectContract { get; set; }
        public bool Activate { get; set; }
        public bool EnableProject { get; set; }
        public int? ContractorsCant { get; set; }
        public decimal? ValorContrato { get; set; }
        public decimal? GastosOperativos { get; set; }
        public decimal? ValorSubTotal { get; set; }
        public string NumberProject { get; set; }
        public string? Project { get; set; }
        public string? StatusContract { get; set; }
        public string? Rubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? NombreRubro { get; set; }

        public DateTime? FechaContrato { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
    }
}
