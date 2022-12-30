using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;

namespace WebApiHiringItm.API.Controllers.ExportToExcel
{
    [ApiController]
    [Authorize]
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

        #region Methods
        [HttpGet("{idContrato}")]
        public async Task<IActionResult> GetViabilidadExcel(int idContrato)
        {
            try
            {
                var result = await _export.ExportToExcelViabilidad(this, idContrato);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                 if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitud Viabilidad.xlsx");
            }
            catch (Exception e)
            {

                throw new Exception("Error", e);
            }
        }

        [HttpGet("{idContrato}")]
        public async Task<IActionResult> GetSolicitudContratacionDap(int idContrato)
        {
            try
            {
                var result = await _export.ExportContratacionDap(this, idContrato);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contratación DAP.xlsx");
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpGet("{idContrato}")]
        
        public async Task<IActionResult> GetSolicitudCdp(int idContrato)
        {
            try
            {
                var result = await _export.ExportCdp(this, idContrato);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitud CDP - DAP.xlsx");
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpGet("{idContrato}")]
        public async Task<IActionResult> GetSolicitudPpa(int idContrato)
        {
            try
            {
                var result = await _export.ExportSolicitudPaa(this, idContrato);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitud PPA.xlsx");
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion
    }
}
