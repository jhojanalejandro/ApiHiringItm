using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contrato
{
    public class AssignmentUserDto
    {
        public Guid Id = Guid.NewGuid();
        public Guid? ContractId { get; set; }
        public Guid UserId { get; set; }
        public Guid RollId { get; set; }
        public Guid AssignmentType { get; set; }
    }
}
