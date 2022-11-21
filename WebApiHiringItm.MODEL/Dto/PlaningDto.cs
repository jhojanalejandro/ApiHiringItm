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
        public string Consecutive { get; set; }
        public int? ProjectFolderId { get; set; }
        public decimal? TotalValue { get; set; }
        public string Objeto { get; set; }
        public int? ContractorCant { get; set; }
    }
}
