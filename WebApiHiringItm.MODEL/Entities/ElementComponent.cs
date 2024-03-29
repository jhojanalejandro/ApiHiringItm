﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class ElementComponent
    {
        public ElementComponent()
        {
            DetailContractor = new HashSet<DetailContractor>();
        }

        public Guid Id { get; set; }
        public string NombreElemento { get; set; }
        public string ObligacionesGenerales { get; set; }
        public string ObligacionesEspecificas { get; set; }
        public Guid? TipoElemento { get; set; }
        public Guid? ComponentId { get; set; }
        public Guid? ActivityId { get; set; }
        public int CantidadContratistas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? ValorTotalContratista { get; set; }
        public decimal? ValorPorDiaContratista { get; set; }
        public decimal ValorPorDia { get; set; }
        public Guid? CpcId { get; set; }
        public bool? Modificacion { get; set; }
        public string Consecutivo { get; set; }
        public string ObjetoElemento { get; set; }
        public string PerfilRequeridoAcademico { get; set; }
        public string PerfilRequeridoExperiencia { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Component Component { get; set; }
        public virtual CpcType Cpc { get; set; }
        public virtual ElementType TipoElementoNavigation { get; set; }
        public virtual ICollection<DetailContractor> DetailContractor { get; set; }
    }
}