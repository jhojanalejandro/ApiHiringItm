using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Contratista
{
    public class FolderDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ContractorId { get; set; }
        public Guid? ContractId { get; set; }
        public string FolderName { get; set; }
        public string? DescriptionProject { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public Guid? FolderType { get; set; }
        public int? Consutive { get; set; }
        public string? FolderDescription { get; set; }
        public string? ContractorName { get; set; }

    }
}
