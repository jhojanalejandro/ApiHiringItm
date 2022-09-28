using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApiHiringItm.CORE.Core.ExcelCore.interfaces;
using WebApiHiringItm.CORE.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Models;
using WebApiRifa.CORE.Helpers;

namespace WebApiHiringItm.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesCore _file;
        private readonly IUploadExcelCore _uploadExcel;

        public FilesController(IFilesCore file, IUploadExcelCore uploadExcel)
        {
            _file = file;
            _uploadExcel = uploadExcel;
        }

        [HttpPost]
        public async Task<IActionResult> Add(FileRequest files)
        {
            try
            {
                var result = await _uploadExcel.ImportarExcel(files);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(FilesDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _file.Create(model);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
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
                var Data = await _file.Delete(id);

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
