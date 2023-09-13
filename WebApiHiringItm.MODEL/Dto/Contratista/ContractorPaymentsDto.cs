using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorPaymentsDto
    {
        public Guid Id { get; set; }
        public string ContractorId { get; set; }
        public string ContractId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DescriptionPayment { get; set; }
        public decimal Paymentcant { get; set; }
        public bool? CashPayment { get; set; }
        public Guid DetailContractor { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Consecutive { get; set; }

    }
}
