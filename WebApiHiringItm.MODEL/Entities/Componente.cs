using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Entities
{
    public partial class Componente
    {
        public int Id { get; set; }
        public string NombreComponente { get; set; } = null!;
        public int IdContrato { get; set; }
    }
}
