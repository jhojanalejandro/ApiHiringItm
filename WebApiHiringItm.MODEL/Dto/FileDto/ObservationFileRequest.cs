using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class ObservationFileRequest
    {
        public Guid? Id { get; set; }
        public Guid FileId { get; set; }
        public string? ReasonRejection { get; set; }
        public string? Observation { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Guid StatusFileId { get; set; }
        public bool Passed { get; set; }
        public Guid? UserId { get; set; }
        public string? ContractId { get; set; }
        public Guid? ContractorId { get; set; }
        public DateTime TermDate { get; set; }

        public List<FileRequest>? Files { get; set; } = null;
    }

    public class FileRequest
    {
        public Guid Id { get; set; }
        public string FilesName { get; set; }
        public Guid? UserId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? Type { get; set; }
        public string? DocumentTypes { get; set; }
        public string? Origin { get; set; }
    }
}
