using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ComponentDto
    {
        public int Id { get; set; }
        public int? PlaningId { get; set; }
        public int? ContractorCant { get; set; }
        public decimal? UnitValue { get; set; }
        public decimal? TotalValue { get; set; }
    }
}
