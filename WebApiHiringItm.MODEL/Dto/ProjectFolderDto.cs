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
        public int IdUser { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
