using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class EconomicdataContractorDto
    {
        public Guid Id { get; set; }
        public Guid? ContractorId { get; set; }
        public Guid? ContractId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UnitValue { get; set; }
        public decimal TotalPayMonth { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public Guid DetailContractorId { get; set; }
        public bool? CashPayment { get; set; }
        public decimal? Missing { get; set; }
        public decimal Debt { get; set; }
        public decimal? Freed { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int Consecutive { get; set; }


    }
}
