using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.PdfDto
{
    public class PreviusStudyDto
    {
        public Guid? Id { get; set; }
        public string? ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorIdentification { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? ContractInitialDate { get; set; }
        public DateTime? ContractFinalDate { get; set; }
        public string SpecificObligations { get; set; }
        public string GeneralObligations { get; set; }
        public string ElementObject { get; set; }
        public decimal TotalValue { get; set; }
        public string MinuteNumber { get; set; }
        public string UnifiedProfile { get; set; }
        public string? RequiredProfile { get; set; }
        public string? RequiredProfileAcademic { get; set; }
        public string? RequiredProfileExperience { get; set; }
        public string? ActivityContractor { get; set; }
        public string? DutyContract { get; set; }
        public bool? PoliceRequire { get; set; }
        public bool? LegalprocessAprove { get; set; }

    }


}
