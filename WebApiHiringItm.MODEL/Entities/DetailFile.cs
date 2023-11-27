﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class DetailFile
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public string ReasonRejection { get; set; }
        public string Observation { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid StatusFileId { get; set; }
        public bool? Passed { get; set; }
        public Guid? UserId { get; set; }
        public Guid ContractorId { get; set; }

        public virtual Contractor Contractor { get; set; }
        public virtual Files File { get; set; }
        public virtual StatusFile StatusFile { get; set; }
        public virtual UserT User { get; set; }
    }
}