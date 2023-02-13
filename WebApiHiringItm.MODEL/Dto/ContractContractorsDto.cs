using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ContractContractorsDto
    {
        public Guid contractId { get; set; }    
        public Guid[] contractors { get; set; }
    }
}
