using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ProjectFolderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string DescriptionProject { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public decimal? Budget { get; set; }
        public int? ContractCant { get; set; }
        public bool? Execution { get; set; }
        public string Cpc { get; set; }
        public string NombreCpc { get; set; }
        public bool? Activate { get; set; }
    }
}
