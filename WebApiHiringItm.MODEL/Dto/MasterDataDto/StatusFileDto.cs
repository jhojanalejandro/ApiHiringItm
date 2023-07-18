using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.MasterDataDto
{
    public class StatusFileDto
    {
        public Guid Id { get; set; }
        public string StatusFileDescription { get; set; }
        public int? ConsecutiveStatus { get; set; }
        public string Code { get; set; }
    }
}
