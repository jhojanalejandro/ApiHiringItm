using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class HiringDataDto
    {
        public int Id { get; set; }
        public int? ContractorId { get; set; }
        public int? UserId { get; set; }
        public DateTime? FechaFinalizacionConvenio { get; set; }
        public string Actividad { get; set; }
        public string Contrato { get; set; }
        public string Compromiso { get; set; }
        public DateTime? FechaExaPreocupacional { get; set; }
        public DateTime? FechaInicioAmpliacion { get; set; }
        public DateTime? FechaDeTerminacionAmpliacion { get; set; }
        public string InterventorItm { get; set; }
        public string CargoInterventorItm { get; set; }
        public string NoAdicion { get; set; }
        public DateTime? FechaDeInicioProyectado { get; set; }
        public DateTime? FechaRealDeInicio { get; set; }
        public string? Ejecucion { get; set; }
        public string? ActaComite { get; set; }
        public DateTime? FechaDeComite { get; set; }
        public bool? RequierePoliza { get; set; }
        public string NoPoliza { get; set; }
        public DateTime? VigenciaInicial { get; set; }
        public DateTime? VigenciaFinal { get; set; }
        public DateTime? FechaExpedicionPoliza { get; set; }
        public decimal? ValorAsegurado { get; set; }
        public int? Nivel { get; set; }
        public string? Rubro { get; set; }
        public string? NombreRubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? Cdp { get; set; }
        public int[] idsContractors { get; set; }
    }
}
