using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class ChargeAccountDto
    {
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string ExpeditionIdentification { get; set; }
        public string Direction { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? PeriodExecutedInitialDate { get; set; }
        public DateTime? PeriodExecutedFinalDate { get; set; }
        public string ElementName { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string BankingEntity { get; set; }
        public decimal TotalValue { get; set; }
        public string ContractName { get; set; }
        public int? ChargeAccountNumber { get; set; }
    }
}
