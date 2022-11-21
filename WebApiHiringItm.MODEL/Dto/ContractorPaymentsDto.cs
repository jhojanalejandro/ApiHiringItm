using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ContractorPaymentsDto
    {
        public int Id { get; set; }
        public int? ContractorId { get; set; }
        public int? UserId { get; set; }
        public decimal? MonthPayment { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string DescriptionPayment { get; set; }
        public decimal? Paymentcant { get; set; }
        public bool? CashPayment { get; set; }
    }
}
