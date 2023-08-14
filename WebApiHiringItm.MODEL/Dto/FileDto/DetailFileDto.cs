using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class DetailFileDto
    {
        public Guid? Id { get; set; }
        public Guid FileId { get; set; }
        public string? Reason { get; set; }
        public string? Observation { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Guid StatusFileId { get; set; }
        public bool Passed { get; set; }
        public Guid? UserId { get; set; }
        public string? ContractId { get; set; }
        public string? ContractorId { get; set; }
        public DateTime TermDate { get; set; }

        public List<FilesDto>? Files { get; set; } = null;
    }
}