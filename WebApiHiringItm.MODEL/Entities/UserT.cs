﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class UserT
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int IdRoll { get; set; }
        public string UserEmail { get; set; }
        public bool Permission { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Roll IdRollNavigation { get; set; }
    }
}