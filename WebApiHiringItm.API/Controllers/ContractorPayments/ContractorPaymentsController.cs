using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.ContractorPaymentsCore.Interface;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.API.Controllers.ContractorPayments
{
    [ApiController]
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
        public async Task<IActionResult> GetById(int id)
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
        public async Task<IActionResult> Add(ContractorPaymentsDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contractorPayment.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(ContractorPaymentsDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contractorPayment.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
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
