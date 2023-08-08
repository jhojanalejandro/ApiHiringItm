using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Models
{
    public class SaveFileResponse
    {
        public Guid FileId { get; set; }
        public bool FileExist { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid UserId { get; set; }

    }
}
