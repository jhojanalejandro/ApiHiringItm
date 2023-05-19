using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class AsignElementOrCompoenteDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Guid[] ContractorId { get; set; }
        public Guid ContractId { get; set; }
        public string? ActivityId { get; set; }

    }
}
