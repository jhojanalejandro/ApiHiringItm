using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.MasterDataDto
{
    public class ElementTypeDto
    {
        public Guid Id { get; set; }
        public string ElementTypeDescription { get; set; }
        public string ElementDescription { get; set; }
        public string Code { get; set; }
    }
}
