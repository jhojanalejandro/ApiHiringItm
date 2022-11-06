using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class PlaningDto
    {
        public int Id { get; set; }
        public string Consecutivo { get; set; }
        public int? ProjectFolderId { get; set; }
        public string Profesional { get; set; }
        public string Laboral { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Objeto { get; set; }
        public int? DayCant { get; set; }
        public int? ContractorCant { get; set; }
    }
}
