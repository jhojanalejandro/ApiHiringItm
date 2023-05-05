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
    public class ContractorPaymentsController : ControllerBase
    {
        private readonly IContractorPaymentsCore _contractorPayment;

        public ContractorPaymentsController(IContractorPaymentsCore contractorPayment)
        {
            _contractorPayment = contractorPayment;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contractorPayment.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contractorPayment.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<ContractorPaymentsDto> model)
        {
            try
            {
                var Data = await _contractorPayment.Create(model);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contractorPayment.Delete(id);

                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
