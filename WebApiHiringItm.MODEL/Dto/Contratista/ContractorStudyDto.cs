using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class ContractorStudyDto
    {

        public Guid Id { get; set; }
        public string TypeStudy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public string DescriptionStudy { get; set; }
        public string InstitutionName { get; set; }
    }
}
