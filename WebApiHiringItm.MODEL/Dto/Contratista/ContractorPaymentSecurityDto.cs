using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorPaymentSecurityDto
    {
        public Guid Id { get; set; }
        public Guid ContractorPayments { get; set; }
        public string Observation { get; set; }
        public decimal PaymentPension { get; set; }
        public decimal PaymentArl { get; set; }
        public decimal PaymentHealth { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Consecutive { get; set; }
    }
}
