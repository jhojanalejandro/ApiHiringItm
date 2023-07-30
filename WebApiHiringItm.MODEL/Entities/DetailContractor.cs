﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class DetailContractor
    {
        public DetailContractor()
        {
            ChangeContractContractor = new HashSet<ChangeContractContractor>();
            ContractorPayments = new HashSet<ContractorPayments>();
            TermContract = new HashSet<TermContract>();
        }

        public Guid Id { get; set; }
        public Guid? ContractId { get; set; }
        public Guid? ContractorId { get; set; }
        public Guid? HiringDataId { get; set; }
        public Guid? ElementId { get; set; }
        public Guid? ComponentId { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? StatusContractor { get; set; }
        public Guid? Economicdata { get; set; }
        public int? Consecutive { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Component Component { get; set; }
        public virtual ContractFolder Contract { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual EconomicdataContractor EconomicdataNavigation { get; set; }
        public virtual ElementComponent Element { get; set; }
        public virtual HiringData HiringData { get; set; }
        public virtual StatusContractor StatusContractorNavigation { get; set; }
        public virtual ICollection<ChangeContractContractor> ChangeContractContractor { get; set; }
        public virtual ICollection<ContractorPayments> ContractorPayments { get; set; }
        public virtual ICollection<TermContract> TermContract { get; set; }
    }
}