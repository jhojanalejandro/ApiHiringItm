using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ContractContractorsDto
    {
        public string TypeMinute { get; set; }
        public string contractId { get; set; }    
        public string[] contractors { get; set; }
    }
}
