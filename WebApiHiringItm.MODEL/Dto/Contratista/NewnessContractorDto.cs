using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class NewnessContractorDto
    {
        public Guid? Id { get; set; }
        public string ContractorId { get; set; }
        public string NewnessDescripcion { get; set; }
        public string NewnessType { get; set; }
        public string ContractId { get; set; }
        public string? NewnessCode { get; set; }

    }
}
