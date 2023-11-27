using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Models
{
    public class EconomicDataRequest
    {
        public List<Guid> Contractors { get; set; }
        public string ContractId { get; set; }
    }
}
