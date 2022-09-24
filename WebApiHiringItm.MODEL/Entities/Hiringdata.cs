﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class HiringData
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdContractor { get; set; }
        public string Convenio { get; set; }
        public string Entidad { get; set; }
        public string Objetoconvenio { get; set; }
        public DateTime? Fechainicioconvenio { get; set; }
        public DateTime? Fechafinalizaciónconvenio { get; set; }
        public string Componente { get; set; }
        public string Talentohumano { get; set; }
        public string Actividad { get; set; }
        public string Objeto { get; set; }
        public string Contrato { get; set; }
        public string Compromiso { get; set; }
        public string Valortotaldelcontrato { get; set; }
        public string Consecutivo { get; set; }
        public string Rubropresupuestal { get; set; }
        public string Nombredelrubro { get; set; }
        public string Cpc { get; set; }
        public string Nombrecpc { get; set; }
        public DateTime? Fechaexapreocupacionales { get; set; }
        public DateTime? Fechainicioampliacion { get; set; }
        public DateTime? Fechadeterminaciónampliacion { get; set; }
        public string Interventoritm { get; set; }
        public string Cargointerventoritm { get; set; }
        public string Noadicion { get; set; }
        public DateTime? Fechadecontrato { get; set; }
        public DateTime? Fechadeinicioproyectado { get; set; }
        public DateTime? Fecharealdeinicio { get; set; }
        public DateTime? Fechafinalizacion { get; set; }
        public decimal? Honorariosmensuales { get; set; }
        public string Encabezado { get; set; }
        public string Ejecucion { get; set; }
        public string Obligacionesgenerales { get; set; }
        public string Obligacionesespecificas { get; set; }
        public string Profesional { get; set; }
        public string Laboral { get; set; }
        public string Actacomite { get; set; }
        public DateTime? Fechadecomite { get; set; }
        public bool? Requierepoliza { get; set; }
        public string Entidadaseguradora { get; set; }
        public string Nopoliza { get; set; }
        public DateTime? Vigenciainicial { get; set; }
        public DateTime? Vigenciafinal { get; set; }
        public DateTime? Fechaexpedicionpoliza { get; set; }
        public decimal? Valorasegurado { get; set; }
        public int? Nivel { get; set; }

        public virtual Contractor IdContractorNavigation { get; set; }
        public virtual UserT IdUserNavigation { get; set; }
    }
}