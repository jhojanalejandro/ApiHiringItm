using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.API.Controllers.Component
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class ElementosComponenteController : ControllerBase
    {
        #region Fields
        private readonly IElementosComponenteCore _element;
        #endregion

        #region Builder
        public ElementosComponenteController(IElementosComponenteCore element)
        {
            _element = element;
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<IActionResult> SaveElement(ElementComponentDto model)
        {
            try
            {
                var isSuccess = await _element.SaveElement(model);
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
        [Route("{id}")]
        public async Task<IActionResult> GetElementsByComponent(Guid id)
        {
            try
            {
                var res = await _element.GetElementsByComponent(id);
                return res.Count == 0 ? BadRequest() : Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var res = await _element.GetById(id);
                return res != null ? Ok(res) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion
    }
}
