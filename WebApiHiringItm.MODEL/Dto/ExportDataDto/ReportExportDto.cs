using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.ExportDataDto
{
    public class ReportExportDto
    {
        public string? ProjecName { get; set; }
        public string? contractorNames { get; set; }
        public string? ContractorLastNames { get; set; }
        public string? ContractorIdentification { get; set; }
        public string? ContractorMail { get; set; }
        public string? phoneNumber { get; set; }
        public string? JuridicProcess { get; set; }
        public string? Contractualprocess { get; set; }
        public string? Comite { get; set; }
        public string? Minute { get; set; }
        public string? PreviusStudy { get; set; }
    }
}
