using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class DetailFileDto
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public string Motivo { get; set; }
        public string Observation { get; set; }

        public List<FilesDto> Files { get; set; } = null;
    }
}