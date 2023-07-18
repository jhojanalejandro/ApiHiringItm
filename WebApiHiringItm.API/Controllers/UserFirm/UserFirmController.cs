using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.HiringDataCore.Interface;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;

namespace WebApiHiringItm.API.Controllers.UserFirm
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserFirmController : ControllerBase
    {
        private readonly IUserFirmCore _userFirm;

        public UserFirmController(IUserFirmCore userFirm)
        {
            _userFirm = userFirm;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Data = await _userFirm.GetAllFirms();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRolls()
        {
            try
            {
                var Data = await _userFirm.GetAllRolls();
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
                var Data = await _userFirm.GetByIdFirm(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserDocument(UserFileDto model)
        {
            try
            {
                var Data = await _userFirm.SaveUserDocument(model);
                return Data == true ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _userFirm.DeleteFirm(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypeUserFile()
        {
            try
            {
                var Data = await _userFirm.GetAllTypeUserFile();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}
