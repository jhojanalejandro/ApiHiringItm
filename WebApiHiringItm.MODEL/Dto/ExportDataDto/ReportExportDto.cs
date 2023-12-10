using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ExportDataDto
{
    public class ReportExportDto
    {
        public string? Consecutive { get; set; }
        public string? ContractorLastName { get; set; }
        public string? contractorNames { get; set; }
        public string? ContractorLastNames { get; set; }
        public string? ContractorIdentification { get; set; }
        public string? ContractorMail { get; set; }
        public string? JuridicProcess { get; set; }
        public string? LegalProccess { get; set; }
        public string? HiringStatus { get; set; }
        public string? MinuteGnenerated { get; set; }
        public string? ComiteGenerated { get; set; }

        public string? Contractualprocess { get; set; }
        public string? Comite { get; set; }
        public string? Minute { get; set; }
        public string? PreviusStudy { get; set; }
    }
}
