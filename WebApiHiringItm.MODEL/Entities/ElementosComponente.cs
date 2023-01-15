﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class ElementosComponente
    {
        public ElementosComponente()
        {
            DetailProjectContractor = new HashSet<DetailProjectContractor>();
        }

        public int Id { get; set; }
        public string NombreElemento { get; set; }
        public string ObligacionesGenerales { get; set; }
        public string ObligacionesEspecificas { get; set; }
        public string TipoElemento { get; set; }
        public int IdComponente { get; set; }
        public int CantidadContratistas { get; set; }
        public int CantidadDias { get; set; }
        public decimal ValorUnidad { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal? ValorTotalContratista { get; set; }
        public decimal? ValorPorDiaContratista { get; set; }
        public decimal? Recursos { get; set; }
        public decimal ValorPorDia { get; set; }
        public string Cpc { get; set; }
        public string NombreCpc { get; set; }
        public int? IdDetalle { get; set; }
        public bool? Modificacion { get; set; }
        public string Consecutivo { get; set; }
        public string ObjetoElemento { get; set; }

        public virtual Componente IdComponenteNavigation { get; set; }
        public virtual ICollection<DetailProjectContractor> DetailProjectContractor { get; set; }
    }
}