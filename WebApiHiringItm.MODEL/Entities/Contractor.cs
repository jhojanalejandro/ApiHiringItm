﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class Contractor
    {
        public Contractor()
        {
            ContractorPayments = new HashSet<ContractorPayments>();
            DetailProjectContractor = new HashSet<DetailProjectContractor>();
            EconomicdataContractor = new HashSet<EconomicdataContractor>();
            FolderContractor = new HashSet<FolderContractor>();
            HiringData = new HashSet<HiringData>();
            NewnessContractor = new HashSet<NewnessContractor>();
        }

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
        public string Estado { get; set; }
        public Guid UserId { get; set; }
        public string ClaveUsuario { get; set; }
        public Guid ContractId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string ObjetoConvenio { get; set; }
        public bool? Habilitado { get; set; }
        public Guid? RollId { get; set; }

        public virtual ProjectFolder Contract { get; set; }
        public virtual UserT User { get; set; }
        public virtual ICollection<ContractorPayments> ContractorPayments { get; set; }
        public virtual ICollection<DetailProjectContractor> DetailProjectContractor { get; set; }
        public virtual ICollection<EconomicdataContractor> EconomicdataContractor { get; set; }
        public virtual ICollection<FolderContractor> FolderContractor { get; set; }
        public virtual ICollection<HiringData> HiringData { get; set; }
        public virtual ICollection<NewnessContractor> NewnessContractor { get; set; }
    }
}