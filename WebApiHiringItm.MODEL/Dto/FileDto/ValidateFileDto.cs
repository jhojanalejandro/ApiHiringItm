using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.FileDto
{
    public class ValidateFileDto
    {
        public bool? Secop { get; set; }
        public bool Exam { get; set; }
        public bool Hv { get; set; }
        public bool ActivateTermContract { get; set; }
        public bool ActivateTermPayments { get; set; }

    }
}
