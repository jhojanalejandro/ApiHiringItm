﻿using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.Contratista;
using WebApiHiringItm.MODEL.Dto.Security;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.API.Controllers.Contractor
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ContractorPaymentsController : ControllerBase
    {
        private readonly IContractorPaymentsCore _contractorPayment;

        public ContractorPaymentsController(IContractorPaymentsCore contractorPayment)
        {
            _contractorPayment = contractorPayment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Data = await _contractorPayment.GetAll();
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
                var Data = await _contractorPayment.GetById(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveContractorPayment(List<ContractorPaymentsDto> modelContractorPaymentsDto)
        {
            try
            {
                var isSuccess = await _contractorPayment.SaveContractorPayment(modelContractorPaymentsDto);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteContractorPayment(string id)
        {
            try
            {
                var isSuccess = await _contractorPayment.DeleteContractorPayment(id);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsContractorList(string contractId, string contractorId)
        {
            try
            {
                var isSuccess = await _contractorPayment.GetPaymentsContractorList(contractId, contractorId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetEmptityHealthContractor(string contractorId)
        {
            try
            {
                var isSuccess = await _contractorPayment.GetEmptityHealthContractor(contractorId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }


        [HttpGet]
        public async Task<IActionResult> ChargeAccountGetById(string contractId, string ContractorId)
        {
            try
            {
                var isSuccess = await _contractorPayment.GetChargeAccount(contractId, ContractorId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {
                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveContractorPaymentSecurity(ContractorPaymentSecurityDto contractorPaymentSecurity)
        {
            try
            {
                var isSuccess = await _contractorPayment.SaveContractorSecurity(contractorPaymentSecurity);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetContractorPaymentSecurity(string contractId)
        {
            try
            {
                var isSuccess = await _contractorPayment.GetContractorSecurity(contractId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetContractorListNomina(string contractId)
        {
            try
            {
                var isSuccess = await _contractorPayment.GetContractorNomina(contractId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {

                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentsContractor(string ContractorId)
        {
            try
            {
                var isSuccess = await _contractorPayment.GetPaymentsContractors(ContractorId);
                if (isSuccess.Success)
                {
                    return Ok(isSuccess);
                }
                else
                {
                    return BadRequest(isSuccess);
                }
            }
            catch (Exception ex)
            {
                var response = ApiResponseHelper.CreateErrorResponse<string>(ex.Message);
                return BadRequest(response);
            }
        }

    }
}
