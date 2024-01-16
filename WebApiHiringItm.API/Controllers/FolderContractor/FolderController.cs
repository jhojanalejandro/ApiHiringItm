using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.Contratista;

namespace WebApiHiringItm.API.Controllers.Folder
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderContractorCore _folder;

        public FolderController(IFolderContractorCore folder)
        {
            _folder = folder;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFolderById(Guid contractorId, Guid contractId)
        {
            try
            {
                var Data = await _folder.GetAllFolderById(contractorId, contractId);
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
                var Data = await _folder.GetById(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveFolderContract(FolderDto model)
        {
            try
            {
                var Data = await _folder.SaveFolderContract(model);

                return Data == true ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(FolderDto model)
        {
            try
            {
                var Data = await _folder.SaveFolderContract(model);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string folderId)
        {
            try
            {
                var isSuccess = await _folder.Delete(folderId);
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
