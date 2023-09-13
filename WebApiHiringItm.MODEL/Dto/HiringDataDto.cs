using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class HiringDataDto
    {
        public Guid? Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid ContractorId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? FechaFinalizacionConvenio { get; set; }
        public string? Contrato { get; set; }
        public string? Compromiso { get; set; }
        public DateTime? FechaExaPreocupacional { get; set; }
        public string? SupervisorItm { get; set; }
        public string? CargoSupervisorItm { get; set; }
        public string? IdentificacionSupervisor { get; set; }        
        public DateTime? FechaRealDeInicio { get; set; }
        public DateTime? FechaDeComite { get; set; }
        public bool? RequierePoliza { get; set; }
        public string? NoPoliza { get; set; }
        public DateTime? VigenciaInicial { get; set; }
        public DateTime? VigenciaFinal { get; set; }
        public DateTime? FechaExpedicionPoliza { get; set; }
        public decimal? ValorAsegurado { get; set; }
        public int? Nivel { get; set; }
        public string? NombreRubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? Cdp { get; set; }
        public string? NumeroActa { get; set; }
        public string? Caso { get; set; }
        public string? StatusContractor { get; set; }
        public string? SupervisorId { get; set; }
        public string ActividadContratista { get; set; }

    }
}
