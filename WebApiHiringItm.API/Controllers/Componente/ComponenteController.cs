﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Componentes.Interfaces;
using WebApiHiringItm.MODEL.Dto.Componentes;

namespace WebApiHiringItm.API.Controllers.Componente
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var res = await _componente.Get(id);
                return res.Count == 0 ? BadRequest() : Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}