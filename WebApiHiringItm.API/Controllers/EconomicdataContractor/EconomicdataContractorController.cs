using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.EconomicdataContractorCore.Interface;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.API.Controllers.EconomicdataContractor
{
    [ApiController]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _economicData.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(EconomicdataContractorDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _economicData.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(EconomicdataContractorDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _economicData.Create(model);

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
                var Data = await _economicData.Delete(id);

                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
