using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.API.Controllers.HiringData
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class HiringDataController : ControllerBase
    {
        private readonly IHiringDataCore _hiringData;

        public HiringDataController(IHiringDataCore hiringData)
        {
            _hiringData = hiringData;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _hiringData.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdHinringData(Guid contractorId, Guid contractId)
        {
            try
            {
                var Data = await _hiringData.GetByIdHinringData(contractorId, contractId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveHiring(List<HiringDataDto> model)
        {
            try
            {
                var Data = await _hiringData.SaveHiringData(model);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _hiringData.Delete(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

    }
}
