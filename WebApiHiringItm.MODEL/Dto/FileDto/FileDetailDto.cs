using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class FileDetailDto
    {
        public Guid Id { get; set; }
        public string FilesName { get; set; }
        public string Filedata { get; set; }
        public string FileType { get; set; }
        public string DescriptionFile { get; set; }
        public string AsistencialUser { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string? MonthPayment { get; set; }
        public bool? Passed { get; set; }
        public string? Type { get; set; }
        public string? DocumentTypes { get; set; }
        public string? StatusPayment { get; set; }
        public string? DocumentTypesCode { get; set; }
        public string? Reason { get; set; }
        public string? Observation { get; set; }
        public string? UserContractual { get; set; }

        public DetailFileDto? DetailFile { get; set; } = null;

    }
}
