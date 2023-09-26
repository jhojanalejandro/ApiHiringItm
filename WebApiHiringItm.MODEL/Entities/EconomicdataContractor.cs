﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class EconomicdataContractor
    {
        public EconomicdataContractor()
        {
            ChangeContractContractor = new HashSet<ChangeContractContractor>();
            ContractorPayments = new HashSet<ContractorPayments>();
        }

        public Guid Id { get; set; }
        public Guid DetailContractorId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UnitValue { get; set; }
        public decimal TotalPayMonth { get; set; }
        public bool? CashPayment { get; set; }
        public decimal? Missing { get; set; }
        public decimal Debt { get; set; }
        public decimal? Freed { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int Consecutive { get; set; }

        public virtual DetailContractor DetailContractor { get; set; }
        public virtual ICollection<ChangeContractContractor> ChangeContractContractor { get; set; }
        public virtual ICollection<ContractorPayments> ContractorPayments { get; set; }
    }
}