using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.API.Controllers.Componente
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class ComponenteController : ControllerBase
    {
        #region Fields
        private readonly IComponenteCore _componente;
        #endregion

        #region Builder
        public ComponenteController(IComponenteCore componente)
        {
            _componente = componente;
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
            var res = await _componente.Get(id);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {

            try
            {
                var res = await _componente.GetActivity(id);
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
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var res = await _componente.GetById(id);
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
