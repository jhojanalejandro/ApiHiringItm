using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ContractorDto
    {
        public int Id { get; set; }
        public string Nro { get; set; }
        public string Convenio { get; set; }
        public string Entidad { get; set; }
        public string ObjetoConvenio { get; set; }
        public DateTime? FechaInicioConvenio { get; set; }
        public DateTime? FechaFinalizacionConvenio { get; set; }
        public string Componente { get; set; }
        public string TalentoHumano { get; set; }
        public string NombreCompleto { get; set; }
        public string DocumentoDeIdentificacion { get; set; }
        public string LugarDeExpedicion { get; set; }
        public string Bachiller { get; set; }
        public string Tecnico { get; set; }
        public string Tecnologo { get; set; }
        public string Pregrado { get; set; }
        public string Especializacion { get; set; }
        public string Maestria { get; set; }
        public string Doctorado { get; set; }
        public string Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Comuna { get; set; }
        public string Barrio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Eps { get; set; }
        public string Pension { get; set; }
        public string Arl { get; set; }
        public string CuentaBancaria { get; set; }
        public string TipoDeCuenta { get; set; }
        public string EntidadCuentaBancaria { get; set; }
        public int UserId { get; set; }
        public int ContractId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ClaveUsuario { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
