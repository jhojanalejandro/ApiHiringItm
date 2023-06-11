using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.FoldersContractorCore.Interface;
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
        public async Task<IActionResult> GetAll(Guid contractorId, Guid contractId)
        {
            try
            {
                var Data = await _folder.GetAllById(contractorId, contractId);
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
                //Obtenemos todos los registros.
                var Data = await _folder.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(FolderDto model)
        {
            try
            {
                var Data = await _folder.Create(model);

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
                var Data = await _folder.Create(model);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _folder.Delete(id);

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
