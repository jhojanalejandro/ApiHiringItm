using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class FolderContractorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid? ContractorId { get; set; }
        public Guid? ContractId { get; set; }
        public string FolderName { get; set; }
        public string DescriptionProject { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
