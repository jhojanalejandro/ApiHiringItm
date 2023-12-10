using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.MessageDto
{
    public class SendMessageObservationDto
    {
        public string ContractId { get; set; }
        public Guid ContractorId { get; set; }
        public string UserId { get; set; }
        public string Observation { get; set; }
        public List<string> Documents { get; set; } = new();
        public string novedad { get; set; }
        public DateTime TermDate { get; set; }

    }
}
