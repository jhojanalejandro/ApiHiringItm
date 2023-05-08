﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Usuario
{
    public class UserFirmDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string FirmData { get; set; }
        public string? UserCharge { get; set; }
        public string? OwnerFirm { get; set; }
        public bool IsOwner { get; set; }

    }
}
