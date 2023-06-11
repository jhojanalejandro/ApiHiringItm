using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.PdfDataCore.InterfaceCore;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.PdfDto;
using WebApiHiringItm.MODEL.Entities;

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


        [HttpGet]
        public async Task<IActionResult> ChargeAccountGetById(string contractId, string ContractorId)
        {
            try
            {
                if (string.IsNullOrEmpty(contractId))
                {
                    return NotFound();
                }
                else
                {
                    var Data = await _pdfData.GetChargeAccount(Guid.Parse(contractId), Guid.Parse(ContractorId));
                    return Data != null ? Ok(Data) : NoContent();
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetDataBill(ContractContractorsDto contractors)
        {
            try
            {
                var Data = await _pdfData.GetDataBill(contractors);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPreviusStudies(ContractContractorsDto contractors)
        {
            try
            {
                var Data = await _pdfData.GetPreviusStudy(contractors);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetCommitteeRequest(ContractContractorsDto contractors)
        {
            try
            {
                var Data = await _pdfData.GetPreviusStudy(contractors);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}
