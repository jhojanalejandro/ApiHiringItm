using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class GetFilesPaymentDto
    {

        public Guid? Id { get; set; }
        public Guid ContractorId { get; set; }
        public Guid? ContractId { get; set; }
        public string FilesName { get; set; }
        public string Filedata { get; set; }
        public string TypeFile { get; set; }
        public string DescriptionFile { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string TypeFilePayment { get; set; }
        public string MonthPayment { get; set; }
        public string? FolderId { get; set; }
        public bool? Passed { get; set; }
    }
}
