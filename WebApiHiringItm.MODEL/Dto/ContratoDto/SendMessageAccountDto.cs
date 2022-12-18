using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class SendMessageAccountDto
    {
        public int IdContrato { get; set; }   
        public int?[] IdContratistas { get; set; }
    }
}
