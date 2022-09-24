﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;

namespace WebApiHiringItm.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HiringDataController : ControllerBase
    {
        private readonly IHiringDataCore _feasibility;

        public HiringDataController(IHiringDataCore feasibility)
        {
            _feasibility = feasibility;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _feasibility.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : (NoContent());
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
                var Data = await _feasibility.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : (NoContent());
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(HiringDataDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _feasibility.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(HiringDataDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _feasibility.Update(model);

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
                var Data = await _feasibility.Delete(id);

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