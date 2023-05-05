using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto
{
    public class ShareDataDto
    {
        public Guid Id { get; set; }
        public string TypeDataShare { get; set; }
        public string DescriptionData { get; set; }
        public string TypedataRegistered { get; set; }
        public string AdditionalData { get; set; }
    }
}
