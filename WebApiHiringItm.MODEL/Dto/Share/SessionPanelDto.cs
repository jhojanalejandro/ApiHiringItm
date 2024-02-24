using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Share
{
    public class SessionPanelDto
    {
        public Guid Id { get; set; }
        public string PanelCode { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? StartSessionDate { get; set; }
        public DateTime? FinalSessionDate { get; set; }
        public bool? Activate { get; set; }
        public bool ActivateSession { get; set;}
        public Guid? ContractId { get; set; }
        public Guid? ContractorId { get; set; }

    }
}
