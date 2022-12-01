using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.MODEL.Dto.ContratoDto;

namespace WebApiHiringItm.API.Controllers.ProjectFolder
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProjectFolderController : ControllerBase
    {
        private readonly IProjectFolder _project;

        public ProjectFolderController(IProjectFolder proeject)
        {
            _project = proeject;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _project.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
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
                var Data = await _project.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProjectFolderDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _project.Create(model);

                //Retornamos datos.
                return Data == true ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(ProjectFolderDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _project.Create(model);

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
                var Data = await _project.Delete(id);

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
