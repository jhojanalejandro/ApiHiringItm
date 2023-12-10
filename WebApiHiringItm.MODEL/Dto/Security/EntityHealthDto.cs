using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Security
{
    public class EntityHealthDto
    {
        public Guid? Id { get; set; }
        public string EntityHealthDescription { get; set; }
        public string Code { get; set; }

    }
}
