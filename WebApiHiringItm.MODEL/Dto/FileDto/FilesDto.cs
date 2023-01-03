using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class FilesDto
    {

        public int? Id { get; set; }
        public int? ContractorId { get; set; }
        public string? FilesName { get; set; }
        public string? Filedata { get; set; }
        public string? TypeFile { get; set; }
        public string? DescriptionFile { get; set; }
        public int UserId { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string? TypeFilePayment { get; set; }
        public string? Mont { get; set; }
        public bool? Passed { get; set; }
        public int? ContractId { get; set; }


    }
}
