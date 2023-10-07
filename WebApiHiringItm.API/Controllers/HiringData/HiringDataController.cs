using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.API.Controllers.HiringData
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class HiringDataController : ControllerBase
    {
        private readonly IHiringDataCore _hiringData;

        public HiringDataController(IHiringDataCore hiringData)
        {
            _hiringData = hiringData;
        }

        #region PUBLI METHODS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Data = await _hiringData.GetAll();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdHinringData(string contractorId, string contractId)
        {
            try
            {
                var isSuccess = await _hiringData.GetByIdHinringData(contractorId, contractId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
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

        [HttpPost]
        public async Task<IActionResult> SaveHiring(List<HiringDataDto> modelHinring)
        {
            try
            {
                var isSuccess = await _hiringData.SaveHiringData(modelHinring);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
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


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var Data = await _hiringData.Delete(id);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDateContractById(string contractorId, string contractId)
        {
            try
            {
                var Data = await _hiringData.GetDateContractById(contractorId, contractId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
        #endregion


    }
}
