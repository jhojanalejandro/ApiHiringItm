using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class MinutaDto
    {
        public HiringDataDto HiringDataDto { get; set; }
        public ElementosComponenteDto elementosComponenteDto { get; set; }
    }
}
