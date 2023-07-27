using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class TermContractDto
    {
        public Guid Id = Guid.NewGuid();
        public Guid DetailContract { get; set; }
        public DateTime FechaTermino { get; set; }
        public DateTime? FechaInicio { get; set; }
        public Guid TermType { get; set; }
    }
}
