using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
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
        public async Task<IActionResult> GetPaymentsContractorList(Guid contractId, Guid contractorId)
        {
            try
            {
                var Data = await _contactor.GetPaymentsContractorList(contractId, contractorId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllByFolder(Guid id)
        {
            try
            {
                var Data = await _contactor.GetByIdFolder(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveDataContractor(ContractorDto model)
        {
            try
            {
                var Data = await _contactor.Create(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(ContractorDto model)
        {
            try
            {
                var Data = await _contactor.Create(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
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
        public async Task<IActionResult> AddNewness(NewnessContractorDto model)
        {
            try
            {
                var Data = await _contactor.AddNewness(model);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
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
        #endregion

    }
}
