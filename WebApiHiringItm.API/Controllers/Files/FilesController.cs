using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.FileDto;
using WebApiHiringItm.MODEL.Models;
using WebApiRifa.CORE.Helpers;

namespace WebApiHiringItm.API.Controllers.Files
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesCore _file;

        public FilesController(IFilesCore file)
        {
            _file = file;
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

        [HttpPost]
        public async Task<IActionResult> AddFileContractor(FilesDto model)
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

        [HttpPost]
        public async Task<IActionResult> AddBillsContractor(FilesDto model)
        {
            try
            {
                var Data = await _file.Addbill(model);
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
                //Obtenemos todos los registros.
                var Data = await _file.CreateDetail(model);

                //Retornamos datos.
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
                var Data = await _file.GetById(id);
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
                //Obtenemos todos los registros.
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

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Data = await _file.Delete(id);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }


    }
}
