using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class FilesDto
    {

        public int? Id { get; set; }
        public Guid ContractorId { get; set; }
        public Guid ContractId { get; set; }
        public string? FilesName { get; set; }
        public string Filedata { get; set; }
        public string TypeFile { get; set; }
        public string? DescriptionFile { get; set; }
        public int? UserId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string TypeFilePayment { get; set; }
        public string? MonthPayment { get; set; }
        public int? FolderId { get; set; }
        public bool? Passed { get; set; }

        public DetailFileDto? DetailFile { get; set; }
    }
}
