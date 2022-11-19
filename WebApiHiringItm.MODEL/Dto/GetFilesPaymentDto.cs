using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class GetFilesPaymentDto
    {
        public int ContractId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Type { get; set; } 
    }
}
