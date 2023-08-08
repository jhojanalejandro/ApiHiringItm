using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class FileContractDto
    {
        public Guid Id { get; set; }
        public Guid? ContractId { get; set; }
        public string FilesName { get; set; }
        public string Filedata { get; set; }
        public string FileType { get; set; }
        public Guid? DocumentType { get; set; }
        public string DescriptionFile { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public Guid? FolderId { get; set; }
        public bool? Passed { get; set; }
        public string? Type { get; set; }
        public string? DocumentTypes { get; set; }
        public Guid? StatusFile { get; set; }
        public string? DocumentTypeCode { get; set; }
        public string? FolderName { get; set; }

    }
}
