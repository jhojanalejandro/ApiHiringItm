using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.API.Controllers.Component
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ComponenteController : ControllerBase
    {
        #region Fields
        private readonly IComponenteCore _componente;
        #endregion

        #region Builder
        public ComponenteController(IComponenteCore Component)
        {
            _componente = Component;
        }
        #endregion

        #region Methods
        [HttpPost]
        public async Task<IActionResult> Add(ComponenteDto model)
        {
            try
            {
                var res = await _componente.Add(model);
                return res != false ? Ok(res) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(ActivityDto model)
        {
            try
            {
                var res = await _componente.AddActivity(model);
                return res != false ? Ok(res) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComponent(Guid id)
        {
            var res = await _componente.GetComponentsByContract(id);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityById(Guid id)
        {

            try
            {
                var res = await _componente.GetActivityById(id);
                return res != null ? Ok(res) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityByComponent(Guid id)
        {

            try
            {
                var res = await _componente.GetActivityByComponent(id);
                return res != null ? Ok(res) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var res = await _componente.Delete(id);
                return res != false ? Ok(res) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetByIdComponent(Guid id, Guid activityId, Guid elementId)
        {
            try
            {
                var res = await _componente.GetByIdComponent(id, activityId,elementId);
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
