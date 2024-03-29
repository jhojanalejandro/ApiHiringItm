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
            AcademicInformation = new HashSet<AcademicInformation>();
            DetailContractor = new HashSet<DetailContractor>();
            DetailFile = new HashSet<DetailFile>();
            Folder = new HashSet<Folder>();
            HiringData = new HashSet<HiringData>();
            NewnessContractor = new HashSet<NewnessContractor>();
        }

        public Guid Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
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
        public string CuentaBancaria { get; set; }
        public string TipoCuenta { get; set; }
        public Guid? EntidadCuentaBancaria { get; set; }
        public Guid UserId { get; set; }
        public string ClaveUsuario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public Guid RollId { get; set; }
        public Guid? Eps { get; set; }
        public Guid? Arl { get; set; }
        public Guid? Afp { get; set; }
        public bool? EnableEdit { get; set; }
        public bool? EnableChangePassword { get; set; }

        public virtual EntityHealth AfpNavigation { get; set; }
        public virtual EntityHealth ArlNavigation { get; set; }
        public virtual Banks EntidadCuentaBancariaNavigation { get; set; }
        public virtual EntityHealth EpsNavigation { get; set; }
        public virtual UserT User { get; set; }
        public virtual ICollection<AcademicInformation> AcademicInformation { get; set; }
        public virtual ICollection<DetailContractor> DetailContractor { get; set; }
        public virtual ICollection<DetailFile> DetailFile { get; set; }
        public virtual ICollection<Folder> Folder { get; set; }
        public virtual ICollection<HiringData> HiringData { get; set; }
        public virtual ICollection<NewnessContractor> NewnessContractor { get; set; }
    }
}