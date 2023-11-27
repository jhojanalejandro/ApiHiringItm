using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class TermContractDto
    {
        public Guid? Id { get; set; }
        public Guid? DetailContractor { get; set; }
        public DateTime TermDate { get; set; }
        public DateTime? StartDate { get; set; }
        public Guid TermType { get; set; }
        public string ContractId { get; set; }
        public string? ContractorId { get; set; }


    }
}
