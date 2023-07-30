using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class CommitteeRequestDto
    {
        public Guid? Id { get; set; }
        public string? ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string ContractNumber { get; set; }
        public string? ContractInitialDate { get; set; }
        public string? ContractFinalDate { get; set; }
        public string User { get; set; }
        public string? UserIdentification { get; set; }
        public string? UserFirm { get; set; }
        public string ElementObject { get; set; }
        public string ElementName { get; set; }
        public decimal? TotalValue { get; set; }
        public string ProfileRequire { get; set; }

    }
}
