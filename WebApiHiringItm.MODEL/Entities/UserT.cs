﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class UserT
    {
        public UserT()
        {
            Contractor = new HashSet<Contractor>();
            ContractorPayments = new HashSet<ContractorPayments>();
            Files = new HashSet<Files>();
            FolderContractor = new HashSet<FolderContractor>();
            HiringData = new HashSet<HiringData>();
            ProjectFolder = new HashSet<ProjectFolder>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Professionalposition { get; set; }
        public string UserPassword { get; set; }
        public int RollId { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Identification { get; set; }

        public virtual Roll Roll { get; set; }
        public virtual ICollection<Contractor> Contractor { get; set; }
        public virtual ICollection<ContractorPayments> ContractorPayments { get; set; }
        public virtual ICollection<Files> Files { get; set; }
        public virtual ICollection<FolderContractor> FolderContractor { get; set; }
        public virtual ICollection<HiringData> HiringData { get; set; }
        public virtual ICollection<ProjectFolder> ProjectFolder { get; set; }
    }
}