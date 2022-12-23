using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class DetalleContratoDto
    {
        public int Id { get; set; }
        public DateTime? FechaContrato { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public bool? Modificacion { get; set; }
        public string TipoContrato { get; set; }
        public int? Idcontrato { get; set; }
        public bool Update { get; set; }

    }
}
