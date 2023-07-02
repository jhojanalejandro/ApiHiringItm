using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorByContractDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string? Identificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string Correo { get; set; }
        public string? StatusContractor { get; set; }
        public bool? Proccess { get; set; }
        public string? ElementId { get; set; }
        public string? ComponentId { get; set; }
        public string? ActivityId { get; set; }
        public string? LegalProccess { get; set; }
        public string? HiringStatus { get; set; }

    }
}
