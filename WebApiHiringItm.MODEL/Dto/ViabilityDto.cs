using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ViabilityDto
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdContractor { get; set; }
        public string Rubropresupuestal { get; set; }
        public string Nombredelrubro { get; set; }
        public string Duracion { get; set; }
        public string Posicion { get; set; }

    }
}
