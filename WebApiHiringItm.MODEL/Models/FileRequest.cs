using Microsoft.AspNetCore.Http;

namespace WebApiHiringItm.MODEL.Models
{
    public class FileRequest
    {
        public int IdUser { get; set; }
        public int IdContrato { get; set; }
        public IFormFile? Excel { get; set; }
    }
}
