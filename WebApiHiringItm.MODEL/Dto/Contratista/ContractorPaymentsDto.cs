using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorPaymentsDto
    {
        public int Id { get; set; }
        public Guid ContractorId { get; set; }
        public Guid ContractId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DescriptionPayment { get; set; }
        public decimal? Paymentcant { get; set; }
        public bool? CashPayment { get; set; }
    }
}
