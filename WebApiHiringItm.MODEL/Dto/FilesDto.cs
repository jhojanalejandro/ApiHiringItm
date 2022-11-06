using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class FilesDto
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int FolderId { get; set; }
        public string FilesName { get; set; }
        public string Filedata { get; set; }
        public string TypeFile { get; set; }
        public string DescriptionFile { get; set; }
        public int UserId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Passed { get; set; }
    }
}
