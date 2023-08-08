using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class MinuteExtensionDto
    {
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string Object { get; set; }
        public decimal TotalValueContract { get; set; }
        public DateTime PeriodInitialDate { get; set; }
        public DateTime PeriodFinalDate { get; set; }
        public string Supervisor { get; set; }
        public string SupervisorIdentification { get; set; }
        public string SupervisorCharge { get; set; }
        public DateTime ExtensionInitialDate { get; set; }
        public DateTime ExtensionFinalDate { get; set; }
        public string ContractNumber { get; set; }
        public string ContractName { get; set; }
        public string Consecutive { get; set; }
        public string NoAdicion { get; set; }
        public string ContractorId { get; set; }

    }
}
