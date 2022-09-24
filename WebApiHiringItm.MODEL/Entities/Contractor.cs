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
            ContractorStudy = new HashSet<ContractorStudy>();
            Files = new HashSet<Files>();
            HiringData = new HashSet<HiringData>();
        }

        public int Id { get; set; }
        public int IdUser { get; set; }
        public int? IdFolder { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documentodeidentificacion { get; set; }
        public string Lugardeexpedicion { get; set; }
        public DateTime? Fechanacimiento { get; set; }
        public string Municipio { get; set; }
        public string Comuna { get; set; }
        public string Barrio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Sexo { get; set; }
        public string Nacionalidad { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Eps { get; set; }
        public string Pension { get; set; }
        public string Arl { get; set; }
        public string Cuentabancaria { get; set; }
        public string Tipodecuenta { get; set; }
        public string Entidadcuentabancaria { get; set; }

        public virtual ProjectFolder IdFolderNavigation { get; set; }
        public virtual UserT IdUserNavigation { get; set; }
        public virtual ICollection<ContractorStudy> ContractorStudy { get; set; }
        public virtual ICollection<Files> Files { get; set; }
        public virtual ICollection<HiringData> HiringData { get; set; }
    }
}