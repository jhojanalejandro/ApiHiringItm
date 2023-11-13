using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class MinutaDto
    {
        public Guid? ContractorId { get; set; }
        public DateTime? FinalContractDate { get; set; }
        public string? Compromiso { get; set; }
        public DateTime? FechaExaPreocupacional { get; set; }
        public DateTime? ComiteDate { get; set; }
        public bool? RequirePolice { get; set; }
        public DateTime? FechaExpedicionPoliza { get; set; }
        public string? Rubro { get; set; }
        public string? RUbroName { get; set; }
        public string? RubroOrigin { get; set; }
        public string? NumeroActa { get; set; }
        public string ElementName { get; set; }
        public string? GeneralObligations { get; set; }
        public string? SpecificObligations { get; set; }
        public decimal UnitValue { get; set; }
        public decimal TotalValueContract { get; set; }
        public decimal? DayValue { get; set; }
        public string? Cpc { get; set; }
        public string? CpcName { get; set; }
        public bool? Mdify { get; set; }
        public string? ElementObject { get; set; }
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string ContractorExpeditionPlace { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Direction { get; set; }
        public string ContractorMail { get; set; }
        public string? ContractNumber { get; set; }
        public bool? ComiteGenerated { get; set; }
        public bool? PreviusStudy { get; set; }
        public DateTime? InitialDateContract { get; set; }

    }
}
