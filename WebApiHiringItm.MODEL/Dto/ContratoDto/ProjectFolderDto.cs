﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class ProjectFolderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string DescriptionProject { get; set; }
        public bool? Execution { get; set; }
        public bool? Activate { get; set; }
        public int? ContractorsCant { get; set; }

        public List<ComponenteDto> Componentes { get; set; }
        //public List<DetalleContratoDto?> DetalleContrato { get; set; }

    }
}
