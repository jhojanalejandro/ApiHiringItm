using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.API.Controllers.EconomicdataContractor
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class EconomicDataContractorController : ControllerBase
    {
        private readonly IEconomicdataContractorCore _economicData;

        public EconomicDataContractorController(IEconomicdataContractorCore economicData)
        {
            _economicData = economicData;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _economicData.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetEconiomicDataById(EconomicDataRequest economicData)
        {
            try
            {
                var Data = await _economicData.GetEconiomicDataById(economicData);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEconomicData(List<EconomicdataContractorDto> modelEconomicData)
        {
            try
            {
                var isSuccess = await _economicData.AddEconomicData(modelEconomicData);
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

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Data = await _economicData.Delete(id);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentByIdContractAndContractor(string contractId, string contractorId)
        {
            try
            {
                var Data = await _economicData.GetPaymentByIdContractAndContractor(contractId, contractorId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}
