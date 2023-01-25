using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class AddPasswordContractorDto
    {
        public Guid Id { get; set; }
        public string Documentodeidentificacion { get; set; }
        public string ClaveUsuario { get; set; }

    }
}
