using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore;
using WebApiHiringItm.MODEL.Dto.PdfDto;

namespace WebApiHiringItm.API.Controllers.PdfData
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class PdfDataController : ControllerBase
    {
        private readonly IPdfDataCore _pdfData;

        public PdfDataController(IPdfDataCore pdfData)
        {
            _pdfData = pdfData;
        }

        [HttpGet]
        public async Task<IActionResult> GetExecutionReport(string contractId, string ContractorId)
        {
            try
            {
                if (string.IsNullOrEmpty(contractId))
                {
                    return NotFound();
                }
                else
                {
                    var Data = await _pdfData.GetExecutionReport(Guid.Parse(contractId), Guid.Parse(ContractorId));
                    return Data != null ? Ok(Data) : NoContent();
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}
