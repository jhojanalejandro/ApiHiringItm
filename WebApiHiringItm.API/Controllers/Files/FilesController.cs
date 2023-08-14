﻿using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Models;
using WebApiRifa.CORE.Helpers;

namespace WebApiHiringItm.API.Controllers.Files
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesCore _file;

        public FilesController(IFilesCore file)
        {
            _file = file;
        }

        #region PUBLIC METOHODS
        [HttpPost]
        public async Task<IActionResult> Update(FilesDto model)
        {
            try
            {
                var Data = await _file.AddFileContractor(model);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFileContractor(FilesDto model)
        {
            try
            {
                var Data = await _file.AddFileContractor(model);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBillsContractor(List<FilesDto> model)
        {
            try
            {
                var isSuccess = await _file.AddbillContractor(model);
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

        [HttpPost]
        public async Task<IActionResult> AddBillsContract(FilesDto model)
        {
            try
            {
                var Data = await _file.AddMinuteGenerateContract(model);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDetailFile(DetailFileDto model)
        {
            try
            {
                var Data = await _file.CreateDetail(model,false,false);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(string id)
        {
            try
            {
                var Data = await _file.GetByIdFile(Guid.Parse(id));
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFileContractorByFolder(Guid contractorId, string folderId, Guid contractId)
        {
            try
            {
                var Data = await _file.GetFileContractorByFolder(contractorId, folderId, contractId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetFileContractByFolder(string folderId, string contractId)
        {
            try
            {
                var Data = await _file.GetFileContractByFolder(folderId, contractId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFileContractorByContract(Guid contractorId, Guid contractId)
        {
            try
            {
                var Data = await _file.GetAllByContract(contractorId, contractId);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllFileContractById(Guid id)
        {
            try
            {
                var Data = await _file.GetAllFileByIdContract(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFileByDatePayments(Guid contractId, string type, string date)
        {
            try
            {
                var Data = await _file.GetAllByDate(contractId, type, date);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> Delete(string fileId)
        {
            try
            {
                var Data = await _file.Delete(fileId);
                return Data != false ? StatusCode(200, Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFileContract(FileContractDto modelFileContract)
        {
            try
            {
                var isSuccess = await _file.AddFileContract(modelFileContract);
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


        [HttpPost]
        public async Task<IActionResult> CreateDetailObservation(DetailFileDto model)
        {
            try
            {
                var Data = await _file.CreateDetailObservation(model);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
        #endregion

    }
}
