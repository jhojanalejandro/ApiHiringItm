﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class GetFileDto
    {
        public Guid ContractId { get; set; }
        public Guid ContractorId { get; set; }
    }
}
