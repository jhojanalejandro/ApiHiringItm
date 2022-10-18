using Microsoft.AspNetCore.Http;

namespace WebApiHiringItm.MODEL.Models
{
    public class FileRequest
    {
        public int UserId { get; set; }
        public int ContractId { get; set; }
        public IFormFile? Excel { get; set; }
    }
}
