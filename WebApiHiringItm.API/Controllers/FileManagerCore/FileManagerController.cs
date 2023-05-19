using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.FileMnager.Interface;

namespace WebApiHiringItm.API.Controllers.FileManagerCore
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class FileManagerController : ControllerBase
    {
        #region BUILDER
        private readonly IFileManagerCore _fileManagerCore;

        public FileManagerController(IFileManagerCore fileManagerCore)
        {
            _fileManagerCore = fileManagerCore;
        }
        #endregion
        #region PUBLIC METHODS
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolderFilesById(Guid id)
        {
            try
            {
                var Data = await _fileManagerCore.GetFolderFilesContract(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContract()
        {
            try
            {
                var Data = await _fileManagerCore.GetAllContract();
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
