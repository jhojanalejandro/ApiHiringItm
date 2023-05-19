using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class MacroMinuteDto
    {
        public string CompanyName { get; set; }
        public string ContractorIdentification { get; set; }
        public string Object { get; set; }
        public decimal? TotalValueContract { get; set; }
        public DateTime? PeriodInitialDate { get; set; }
        public DateTime? PeriodFinalDate { get; set; }
        public string Supervisor { get; set; }
        public string SupervisorIdentification { get; set; }
        public string SupervisorCharge { get; set; }
        public string? ExtensionInitialDate { get; set; }
        public string? ExtensionFinalDate { get; set; }
        public string? ContractNumber { get; set; }
        public string? ContractName { get; set; }
    }
}
