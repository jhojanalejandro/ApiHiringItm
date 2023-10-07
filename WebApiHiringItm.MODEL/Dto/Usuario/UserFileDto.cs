using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiHiringItm.MODEL.Dto.Usuario
{
    public class UserFileDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string FileData { get; set; }
        public Guid? RollId { get; set; }
        public string? OwnerFirm { get; set; }
        public bool IsOwner { get; set; }
        public Guid? UserFileType { get; set; }
        public string FileType { get; set; }
        public string FileNameC { get; set; }

    }
}
