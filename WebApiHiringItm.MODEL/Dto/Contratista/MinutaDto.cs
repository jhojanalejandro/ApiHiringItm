using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Componentes;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class MinutaDto
    {
        public Guid? ContractorId { get; set; }
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
        public DateTime? FechaExpedicionPoliza { get; set; }
        public string? Rubro { get; set; }
        public string? NombreRubro { get; set; }
        public string? FuenteRubro { get; set; }
        public string? Cdp { get; set; }
        public string? NumeroActa { get; set; }
        public string? NombreElemento { get; set; }
        public string? ObligacionesGenerales { get; set; }
        public string? ObligacionesEspecificas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorPorDia { get; set; }
        public string? Cpc { get; set; }
        public string? NombreCpc { get; set; }
        public bool? Modificacion { get; set; }
        public string? Consecutivo { get; set; }
        public string? ObjetoElemento { get; set; }
        public string? TipoContratacion { get; set; }
        public string? Codigo { get; set; }
        public string? Convenio { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public string LugarExpedicion { get; set; }
        public string Especializacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Barrio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string TipoAdministradora { get; set; }
        public string Administradora { get; set; }
        public string CuentaBancaria { get; set; }
        public string TipoCuenta { get; set; }
        public string EntidadCuentaBancaria { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? ObjetoConvenio { get; set; }
        public string? CompanyName { get; set; }
        public string? ProjectName { get; set; }
        public string? DescriptionProject { get; set; }
        public string? NumberProject { get; set; }

    }
}
