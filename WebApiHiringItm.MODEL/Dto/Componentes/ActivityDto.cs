using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ActivityDto
    {
        public Guid? Id { get; set; }
        public string NombreActividad { get; set; }
        public Guid ComponentId { get; set; }

        public List<ElementComponentDto> Elementos { get; set; } = null;

    }
}
