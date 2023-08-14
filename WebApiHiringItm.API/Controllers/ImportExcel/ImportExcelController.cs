using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.ImportExcelCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Entities;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.API.Controllers.ImportExcel
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ImportExcelController : ControllerBase
    {
        private readonly IImportExcelCore _importExcel;
        #region BUILDER
        public ImportExcelController(IImportExcelCore importExcel)
        {
            _importExcel = importExcel;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> AddExcel([FromForm] FileRequest model)
        {
            try
            {
                var result = await _importExcel.ImportarExcel(model);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelCdp([FromForm] FileRequest model)
        {
            try
            {
                var result = await _importExcel.ImportCdp(model);
                return StatusCode(200,result);
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelElement([FromForm] FileRequest model)
        {
            try
            {
                var isSuccess = await _importExcel.ImportElement(model);
                if (isSuccess.Success)
                {
                    var response = ApiResponseHelper.CreateResponse(isSuccess);
                    return Ok(response);
                }
                else
                {
                    var response = ApiResponseHelper.CreateErrorResponse<string>(isSuccess.Message);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }
    }
}
