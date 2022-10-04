using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class FolderContractorDto
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int? IdContractor { get; set; }
        public string FolderName { get; set; }
        public string DescriptionProject { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
