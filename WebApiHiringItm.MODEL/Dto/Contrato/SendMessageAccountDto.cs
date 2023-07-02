using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ContratoDto
{
    public class SendMessageAccountDto
    {
        public Guid ContractId { get; set; }   
        public Guid?[] ContractorsId { get; set; }
        public Guid UserId { get; set; }

    }
}
