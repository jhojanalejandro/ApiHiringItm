using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class HistoryContractorDto
    {
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public string? Identificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

    }
}
