using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class DetailContractDto
    {
        public Guid Id { get; set; }
        public Guid? ContractId { get; set; }
        public DateTime? FechaContrato { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string? NoAdicion { get; set; }
        public Guid? DetailType { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string UserId { get; set; }

        public string? Rubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? StatusContractId { get; set; }
        public int? Consecutive { get; set; }
        public DateTime? RegisterDateContract { get; set; }
        public bool Activate { get; set; }

        public decimal? ResourceContract { get; set; }
        public decimal? ValorContrato { get; set; }
        public decimal? GastosOperativos { get; set; }
        public decimal? ValorSubTotal { get; set; }
        public decimal? RecursosAdicionales { get; set; }
        public decimal? TotalLiberado { get; set; }
        public int? ContractorsCant { get; set; }
    }
}
