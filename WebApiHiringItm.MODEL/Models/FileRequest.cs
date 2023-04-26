using Microsoft.AspNetCore.Http;

namespace WebApiHiringItm.MODEL.Models
{
    public class FileRequest
    {
        public Guid UserId { get; set; }
        public Guid ContractId { get; set; }
        public IFormFile? Excel { get; set; }
    }
}
