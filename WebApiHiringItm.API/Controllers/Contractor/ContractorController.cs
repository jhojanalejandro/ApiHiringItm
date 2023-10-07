using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.Contratista;

namespace WebApiHiringItm.API.Controllers.Contractor
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ContractorController : ControllerBase
    {
        #region BUILDER
        private readonly IContractorCore _contactor;

        public ContractorController(IContractorCore contactor)
        {
            _contactor = contactor;
        }
        #endregion

        #region PUBLIC METHODS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Data = await _contactor.GetAll();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetContractorByContract(string contractId, bool originNomina)
        {
            try
            {
                var isSuccess = await _contactor.GetContractorByContract(contractId, originNomina);
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
        public async Task<IActionResult> SaveDataContractor(PersonalInformation model)
        {
            try
            {
                var Data = await _contactor.SavePersonalInformation(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonalInformation personalInformation)
        {
            try
            {
                var isSuccess = await _contactor.SavePersonalInformation(personalInformation);
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
        public async Task<IActionResult> UpdateAsignment(AsignElementOrCompoenteDto model)
        {
            try
            {
                var Data = await _contactor.UpdateAsignment(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }



        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    try
        //    {
        //        var Data = await _contactor.Delete(id);
        //        return Data != false ? Ok(Data) : NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error", ex);
        //    }
        //}

        [HttpGet("{contractorId}")]
        public async Task<IActionResult> GetContractsByContractor(string contractorId)
        {
            try
            {
                var Data = await _contactor.getContractsByContractor(contractorId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentMinutesPdf(Guid contractId, Guid contractorId)
        {
            try
            {
                var Data = await _contactor.GetDocumentPdf(contractId, contractorId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddNewness(NewnessContractorDto newnessModel)
        {

            try
            {
                var isSuccess = await _contactor.AddNewness(newnessModel);
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
        public async Task<IActionResult> GetHistoryContractor()
        {
            try
            {
                var Data = await _contactor.GetHistoryContractor();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public  IActionResult ValidateDocumentUpload(string contractId, string contractorId)
        {
            try
            {
                var Data =  _contactor.ValidateDocumentUpload(Guid.Parse(contractId), Guid.Parse(contractorId));
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveModifyMinute(ChangeContractContractorDto changeContractModel)
        {
            try
            {
                var isSuccess = await _contactor.SaveModifyMinute(changeContractModel);
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
        public async Task<IActionResult> GetById(string contractorId)
        {
            try
            {
                var Data = await _contactor.GetById(contractorId);
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
