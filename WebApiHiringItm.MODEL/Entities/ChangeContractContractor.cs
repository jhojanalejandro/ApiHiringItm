﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class ChangeContractContractor
    {
        public Guid Id { get; set; }
        public Guid DetailContractorId { get; set; }
        public Guid EconomicdataContractor { get; set; }
        public string ElementName { get; set; }
        public string SpecificObligations { get; set; }
        public string GeneralObligations { get; set; }
        public int? CantDays { get; set; }
        public decimal? ValueDay { get; set; }
        public Guid? CpcId { get; set; }
        public bool? IsModify { get; set; }
        public string ElementObject { get; set; }
        public DateTime? InitialAdditionDate { get; set; }
        public DateTime? FinalAdditionDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid MinuteType { get; set; }
        public string NoAddition { get; set; }
        public int Consecutive { get; set; }
        public bool? IsAddition { get; set; }
        public string PerfilRequeridoAcademico { get; set; }
        public string PerfilRequeridoExperiencia { get; set; }

        public virtual DetailContractor DetailContractor { get; set; }
        public virtual EconomicdataContractor EconomicdataContractorNavigation { get; set; }
        public virtual MinuteType MinuteTypeNavigation { get; set; }
    }
}