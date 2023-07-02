using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.MasterDataDto
{
    public class RubroTypeDto
    {
        public Guid Id { get; set; }
        public string Rubro { get; set; }
        public string RubroNumber { get; set; }
        public string RubroOrigin { get; set; }
    }
}
