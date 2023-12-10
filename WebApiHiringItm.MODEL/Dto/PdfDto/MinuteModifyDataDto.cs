using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class MinuteModifyDataDto
    {
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string Object { get; set; }
        public decimal TotalValueContract { get; set; }
        public DateTime PeriodInitialDate { get; set; }
        public DateTime PeriodFinalDate { get; set; }
        public string Supervisor { get; set; }
        public string? SupervisorIdentification { get; set; }
        public string SupervisorCharge { get; set; }
        public DateTime? ExtensionInitialDate { get; set; }
        public DateTime? ExtensionFinalDate { get; set; }
        public string ContractNumber { get; set; }
        public string ContractName { get; set; }
        public int Consecutive { get; set; }
        public string NoAdicion { get; set; }
        public string ContractorId { get; set; }
        public int? NumberModify { get; set; }
        public string? SpecificObligations { get; set; }
        public string? GeneralObligations { get; set; }
        public string? RubroContract { get; set; }
        public decimal? AdditionValue { get; set; }
        public string? TypeModify { get; set; }
        public DateTime InitialDateContract { get; set; }
        public DateTime FinalDateContract { get; set; }
        public decimal UnitValueContract { get; set; }
        public decimal? InitialValue { get; set; }

    }
}
