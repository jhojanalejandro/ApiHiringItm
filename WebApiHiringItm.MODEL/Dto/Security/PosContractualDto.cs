using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.FileDto;

namespace WebApiHiringItm.MODEL.Dto.Security
{
    public class PosContractualDto
    {
        public Guid Id { get; set; }
        public Guid ContractorPayments { get; set; }
        public string Observation { get; set; }
        public decimal PaymentPension { get; set; }
        public decimal PaymentArl { get; set; }
        public decimal PaymentEps { get; set; }
        public DateTime RegisterDate { get; set; }
        public string PayrollNumber { get; set; }
        public DateTime PaymentPeriodDate { get; set; }
        public string? CorrectArlPayment { get; set; }
        public string? CorrectAfpPayment { get; set; }
        public string? CorrectEpsPayment { get; set; }
        public string CorrectSheet { get; set; }
    }
}
