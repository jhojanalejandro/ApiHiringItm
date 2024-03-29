﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorDto
    {
        public Guid Id { get; set; }
        public string TipoContratacion { get; set; }
        public string Codigo { get; set; }
        public string Convenio { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public string LugarExpedicion { get; set; }
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
        public string Estado { get; set; }
        public Guid UserId { get; set; }
        public string ClaveUsuario { get; set; }
        public Guid ContractId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string ObjetoConvenio { get; set; }
        public Guid? StatusContractor { get; set; }
        public Guid? RollId { get; set; }
    }
}
