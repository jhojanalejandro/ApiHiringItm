using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.API.Controllers.User
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserCore _user;

        public UserController(IUserCore user)
        {
            _user = user;
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _user.Authenticate(model);

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return StatusCode(200, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _user.GetAll();

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
                var Data = await _user.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(UserTDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _user.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(UserTDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _user.Update(model);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UserUpdatePasswordDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _user.UpdatePassword(model);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost("{authToken}")]
        public async Task<IActionResult> ValidateTokens(string authToken)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _user.ValidateT(authToken);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
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
                var Data = await _user.Delete(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> retrievePassword(RetrievePassword model)
        {
            try
            {
                var Data = await _user.GetUserForgetPassword(model);

                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}
