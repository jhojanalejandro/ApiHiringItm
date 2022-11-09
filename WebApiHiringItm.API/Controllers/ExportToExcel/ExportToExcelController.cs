using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;
using WebApiHiringItm.CORE.Core.ExportToExcel.Interfaces;

namespace WebApiHiringItm.API.Controllers.ExportToExcel
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet("GetViabilidadExcel")]
        public async Task<IActionResult> GetViabilidadExcel()
        {
            try
            {
                var result = await _export.ExportToExcelViabilidad(this);
                Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                if (result == null)
                {
                    return NoContent();
                }
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Solicitud Viabilidad.xlsx");
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpGet("GetSolicitudContratacionDap")]
        public async Task<IActionResult> GetSolicitudContratacionDap()
        {
            try
            {
                var result = await _export.ExportContratacionDap(this);
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

        [HttpGet("GetSolicitudCdp")]
        public async Task<IActionResult> GetSolicitudCdp()
        {
            try
            {
                var result = await _export.ExportCdp(this);
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

        [HttpGet("GetSolicitudPpa")]
        public async Task<IActionResult> GetSolicitudPpa()
        {
            try
            {
                var result = await _export.ExportSolicitudPpa(this);
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
