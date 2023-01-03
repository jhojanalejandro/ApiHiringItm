using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class EconomicdataContractorDto
    {
        public int? Id { get; set; }
        public int ContractorId { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? TotalPaidMonth { get; set; }
        public bool? CashPayment { get; set; }
        public decimal? Missing { get; set; }
        public decimal? Debt { get; set; }
        public decimal? Freed { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
