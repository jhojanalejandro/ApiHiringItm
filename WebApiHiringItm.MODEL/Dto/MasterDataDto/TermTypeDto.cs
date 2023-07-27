using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.MasterDataDto
{
    public class TermTypeDto
    {
        public Guid Id { get; set; }
        public string TermDescription { get; set; }
        public string Code { get; set; }
    }
}
