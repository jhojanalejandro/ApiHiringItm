using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Models
{
    public class RequestReportContract
    {
        public string ContractId { get; set; }
        public int? TypeStatus { get; set; }
        public int? Status { get; set; }
    }
}
