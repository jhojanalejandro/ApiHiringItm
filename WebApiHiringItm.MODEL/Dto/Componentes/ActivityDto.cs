﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public string NombreActividad { get; set; }
        public int IdComponente { get; set; }
    }
}