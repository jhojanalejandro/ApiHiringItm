using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.API.Controllers.Contractor
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ContractorController : ControllerBase
    {
        private readonly IContractorCore _contactor;

        public ContractorController(IContractorCore contactor)
        {
            _contactor = contactor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contactor.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllByFolder(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contactor.GetByIdFolder(id);

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
                var Data = await _contactor.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ContractorDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contactor.Create(model);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddExcel([FromForm] FileRequest model)
        {
            try
            {
                var result = await _contactor.ImportarExcel(model);
                return Ok(result);
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
                //Obtenemos todos los registros.
                var Data = await _contactor.Create(model);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendContractorCount(int idContract)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contactor.SendContractorCount(idContract);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contactor.Delete(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _contactor.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return StatusCode(200, response);
        }

    }
}
