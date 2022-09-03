using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class FeasibilityRequestDto
    {
        public int Id { get; set; }
        public string Convenio { get; set; }
        public string Entidad { get; set; }
        public string Componente { get; set; }
        public string Rubropresupuestal { get; set; }
        public string Nombredelrubro { get; set; }
        public string Cpc { get; set; }
        public string Nombrecompleto { get; set; }
        public string Documentodeidentificación { get; set; }
        public string Actividad { get; set; }
        public string Objeto { get; set; }
        public string Valortotaldelcontrato { get; set; }
        public string Consecutivo { get; set; }
        public string Talentohumano { get; set; }
    }
}
