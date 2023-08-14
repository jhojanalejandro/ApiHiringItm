using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ChangeContractContractorDto
    {
        public Guid Id { get; set; }
        public Guid? DetailContractorId { get; set; }
        public Guid? EconomicdataContractorId { get; set; }
        public string? NombreElemento { get; set; }
        public string? ObligacionesGenerales { get; set; }
        public string? ObligacionesEspecificas { get; set; }
        public int? CantidadDias { get; set; }
        public decimal? Recursos { get; set; }
        public decimal? ValorPorDia { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? Debt { get; set; }
        public Guid? CpcId { get; set; }
        public bool? Modificacion { get; set; }
        public string Consecutivo { get; set; }
        public string ObjetoElemento { get; set; }
        public string PerfilRequerido { get; set; }
        public DateTime? FechaInicioAdicion { get; set; }
        public DateTime? FechaFinAdicion { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Guid? MinuteType { get; set; }
        public string NoAdicion { get; set; }
        public int? Consecutive { get; set; }
        public bool? IsAddition { get; set; }
        public string ContractorId { get; set; }
        public string ContractId { get; set; }

    }
}
