﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class DetailContract
    {
        public Guid Id { get; set; }
        public int Consecutive { get; set; }
        public Guid ContractId { get; set; }
        public DateTime? FechaContrato { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public string TipoContrato { get; set; }
        public Guid? DetailType { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public Guid UserId { get; set; }

        public virtual ContractFolder Contract { get; set; }
        public virtual DetailType DetailTypeNavigation { get; set; }
        public virtual UserT User { get; set; }
    }
}