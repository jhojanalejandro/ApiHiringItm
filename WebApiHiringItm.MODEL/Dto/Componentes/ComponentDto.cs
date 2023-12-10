using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.Componentes
{
    public class ComponentDto
    {

        public Guid? Id { get; set; }
        public string NombreComponente { get; set; }
        public Guid ContractId { get; set; }

        public List<ElementComponentDto?> Elementos { get; set; } = null;
        public List<ActivityDto?> Activities { get; set; } = null;

    }
}
