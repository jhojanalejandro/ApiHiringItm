using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class PorcentageDto
    {
        public Guid Id { get; set; }
        public decimal PercentageValue { get; set; }
        public string Code { get; set; }
    }
}
