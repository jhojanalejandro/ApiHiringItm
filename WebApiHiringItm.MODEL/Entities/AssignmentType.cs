﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class AssignmentType
    {
        public AssignmentType()
        {
            AssigmentContract = new HashSet<AssigmentContract>();
        }

        public Guid Id { get; set; }
        public string AssigmentTypeDescription { get; set; }
        public string Code { get; set; }

        public virtual ICollection<AssigmentContract> AssigmentContract { get; set; }
    }
}