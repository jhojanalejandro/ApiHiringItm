﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class RubroType
    {
        public RubroType()
        {
            ContractFolder = new HashSet<ContractFolder>();
        }

        public Guid Id { get; set; }
        public string Rubro { get; set; }
        public string RubroNumber { get; set; }
        public string RubroOrigin { get; set; }

        public virtual ICollection<ContractFolder> ContractFolder { get; set; }
    }
}