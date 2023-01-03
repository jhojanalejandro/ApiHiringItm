using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.CuentaCobroDto
{
    public class CuentaCobroDto
    {

        public int Id { get; set; }
        public string TipoContratacion { get; set; }
        public string Codigo { get; set; }
        public string Convenio { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public string LugarExpedicion { get; set; }
        public string Tecnico { get; set; }
        public string Tecnologo { get; set; }
        public string Pregrado { get; set; }
        public string Especializacion { get; set; }
        public string Maestria { get; set; }
        public string Doctorado { get; set; }
        public string Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
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
        public int ContractId { get; set; }
        public decimal? UnitValue { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Company { get; set; }
        public ContractorPayments Payment { get; set; }

    }
}
