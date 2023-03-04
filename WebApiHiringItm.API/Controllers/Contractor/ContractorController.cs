﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Models;

namespace WebApiHiringItm.API.Controllers.Contractor
{
    [ApiController]
    //[Authorize]
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
                var Data = await _contactor.GetAll();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsContractorList(Guid contractId, Guid contractorId)
        {
            try
            {
                var Data = await _contactor.GetPaymentsContractorList(contractId, contractorId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllByFolder(Guid id)
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

        [HttpGet]
        public async Task<IActionResult> GetById(Guid contractorId, Guid contractId)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _contactor.GetById(contractorId, contractId);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDataBill(ContractContractorsDto contractors)
        {
            try
            {
                var Data = await _contactor.GetDataBill(contractors);
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
                var Data = await _contactor.Create(model);

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
                var Data = await _contactor.Create(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsignment(AsignElementOrCompoenteDto model)
        {
            try
            {
                var Data = await _contactor.UpdateAsignment(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendContractorAccount(SendMessageAccountDto ids)
        {
            try
            {
                var Data = await _contactor.SendContractorCount(ids);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
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
