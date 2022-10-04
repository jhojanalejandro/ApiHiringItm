using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class FilesDto
    {
        public int Id { get; set; }
        public int IdContractor { get; set; }
        public int IdFolder { get; set; }
        public string FilesName { get; set; }
        public string Fildata { get; set; }
        public string TypeFile { get; set; }
        public string DescriptionFile { get; set; }
        public int IdUser { get; set; }
        public DateTime? RegisterDate { get; set; }

    }
}
