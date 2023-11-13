using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Core.MasterDataCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.MasterDataDto;
using WebApiHiringItm.MODEL.Dto.Usuario;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.API.Controllers.MasterData
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataCore _masterDataCore;

        public MasterDataController(IMasterDataCore masterDataCore)
        {
            _masterDataCore = masterDataCore;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentType()
        {
            try
            {
                var Data = await _masterDataCore.GetDocumentType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFileTypes()
        {
            try
            {
                var Data = await _masterDataCore.GetAllFileType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCpcType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllCpcType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusContract()
        {
            try
            {
                var Data = await _masterDataCore.GetSatatusContract();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllElementType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllElementType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusFile()
        {
            try
            {
                var Data = await _masterDataCore.GetStatusFile();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetMinutes()
        {
            try
            {
                var Data = await _masterDataCore.GetAllMinuteType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBanks()
        {
            try
            {
                var Data = await _masterDataCore.GetBanks();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRubros()
        {
            try
            {
                var Data = await _masterDataCore.GetAllRubroType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssignmentType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllAssignmentType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTermType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllTermType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDetailType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllDetailType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetNewnessType()
        {
            try
            {
                var Data = await _masterDataCore.GetNewnessType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmptityHealth()
        {
            try
            {
                var Data = await _masterDataCore.GetEmptityHealth();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveRubro(RubroTypeDto model)
        {
            try
            {
                var isSuccess = await _masterDataCore.SaveRubro(model);
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
        public async Task<IActionResult> SaveBank(BanksDto model)
        {
            try
            {
                var isSuccess = await _masterDataCore.SaveBank(model);
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
        public async Task<IActionResult> SaveCpcType(CpcTypeDto model)
        {
            try
            {
                var isSuccess = await _masterDataCore.SaveCpcType(model);
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
