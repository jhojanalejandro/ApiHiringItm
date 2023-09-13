using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Models
{
    public class MailRequestContractor
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ImageMessage { get; set; }
        public string ContractNumber { get; set; }
        public DateTime TermDate { get; set; }
        public List<FileAttach> FileMessageAttach { get; set; } = new List<FileAttach>();
        public string Documents { get; set; }

    }

    public class FileAttach
    {
        public string FileData { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }

    }
}
