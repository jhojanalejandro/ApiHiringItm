using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contrato
{
    public class ContractorPaymentListDto
    {
        public Guid Id { get; set; }
        public string ContractorId { get; set; }
        public string ContractId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DescriptionPayment { get; set; }
        public string Paymentcant { get; set; }
        public bool? CashPayment { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Consecutive { get; set; }
        public string? LevelRisk { get; set; }
        public string? ProjectName { get; set; }

    }
}
