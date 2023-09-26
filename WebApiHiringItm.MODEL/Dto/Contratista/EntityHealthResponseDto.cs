using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class EntityHealthResponseDto
    {
        public string? IdEps { get; set; }
        public string? IdAfp { get; set; }
        public string? IdArl { get; set; }
        public string Arl { get; set; }
        public string Eps { get; set; }
        public string Afp { get; set; }
        public string CodeArl { get; set; }
        public string CodeEps { get; set; }
        public string CodeAfp { get; set; }

    }
}
