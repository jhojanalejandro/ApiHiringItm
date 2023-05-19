using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class DetalleContratoDto
    {
        public Guid Id { get; set; }
        public Guid? ContractId { get; set; }
        public DateTime? FechaContrato { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string? NoAdicion { get; set; }
        public string? TipoContrato { get; set; }
        public bool Update { get; set; }

    }
}
