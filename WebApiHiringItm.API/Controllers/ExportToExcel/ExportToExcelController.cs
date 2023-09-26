using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.API.Controllers.ExportToExcel
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class ExportToExcelController : ControllerBase
    {
        #region Dependency
        private readonly IExportToExcelCore _export;
        #endregion

        #region Constructor
        public ExportToExcelController(IExportToExcelCore export)
        {
            _export = export;
        }
        #endregion

        #region PUBLIC METHODS

        [HttpGet("{ContractId}")]
        public async Task<IActionResult> GetSolicitudContratacionDap(Guid ContractId)
        {
            try
            {
                var result = await _export.ExportContratacionDap(this, ContractId);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contratación DAP.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{ContractId}")]
        
        public async Task<IActionResult> GetSolicitudCdp(Guid ContractId)
        {
            try
            {
                var result = await _export.ExportCdp(this, ContractId);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitud CDP - DAP.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{ContractId}")]
        public async Task<IActionResult> GetSolicitudPpa(Guid ContractId)
        {
            try
            {
                var result = await _export.ExportSolicitudPaa(this, ContractId);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitud PPA.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }


        [HttpGet("{ContractId}")]
        public async Task<IActionResult> ExportToExcelCdp(Guid ContractId)
        {

            try
            {
                var isSuccess = await _export.ExportToExcelCdp(ContractId);
                if (isSuccess.Success)
                {
                    Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                    return File(isSuccess.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exportar CDP.xlsx");
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {
                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("{ContractId}")]
        public async Task<IActionResult> ExportElement(Guid ContractId)
        {
            try
            {
                var result = await _export.ExportElement(this,ContractId);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exportar elementos.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GenrateReportContract(RequestReportContract requestReportContract)
        {
            try
            {
                var result = await _export.GenerateReport(requestReportContract);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "REPORTE CONTRATO.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSatisfactionReport(SatisfaccionReportRequest satisfaccionReportRequest)
        {
            try
            {
                var result = await _export.GenerateSatisfactionReport(Guid.Parse(satisfaccionReportRequest.contractId), satisfaccionReportRequest.base64);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "REPORTE CONTRATO.xlsx");
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }



        #endregion
    }
}
