using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.FileDto;

namespace WebApiHiringItm.MODEL.Dto.Security
{
    public class ContractorPaymentSecurityDto
    {
        public Guid Id { get; set; }
        public Guid ContractorPayments { get; set; }
        public string Observation { get; set; }
        public decimal PaymentPension { get; set; }
        public decimal PaymentArl { get; set; }
        public decimal PaymentEps { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Consecutive { get; set; }
        public string PayrollNumber { get; set; }
        public DateTime PaymentPeriodDate { get; set; }
        public bool? CorrectArlPayment { get; set; }
        public bool? CorrectAfpPayment { get; set; }
        public bool? CorrectEpsPayment { get; set; }
        public bool CorrectSheet { get; set; }
        public FilesDto ContractorFile { get; set; }

    }
}
