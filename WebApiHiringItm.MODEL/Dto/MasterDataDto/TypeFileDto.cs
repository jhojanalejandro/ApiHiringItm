using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.MasterDataDto
{
    public class TypeFileDto
    {
        public Guid? Id { get; set; }
        public string ElementType { get; set; }
        public string ElementDescription { get; set; }
        public string Code { get; set; }
    }
}
