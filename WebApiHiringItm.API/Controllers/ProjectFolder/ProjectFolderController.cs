using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.MODEL.Dto.ContratoDto;

namespace WebApiHiringItm.API.Controllers.ProjectFolder
{
    [ApiController]
    //[Authorize]
    [Route("[controller]/[action]")]
    public class ProjectFolderController : ControllerBase
    {
        private readonly IProjectFolder _project;

        public ProjectFolderController(IProjectFolder proeject)
        {
            _project = proeject;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(bool inProgress, string tipoModulo)
        {
            try
            {
                List<ProjectFolderDto> projectFolders = new List<ProjectFolderDto>();
                if (inProgress)
                    projectFolders = await _project.GetAllInProgess(tipoModulo);
                else
                    projectFolders = await _project.GetAllActivate();

                return projectFolders != null ? Ok(projectFolders) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
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


        [HttpGet]
        public async Task<IActionResult> GetByIdDetail(Guid id, bool tipoConsulta)
        {
            try
            {
                if (tipoConsulta)
                {
                    var Data = await _project.GetDetailById(id);
                    return Data != null ? Ok(Data) : NoContent();
                }
                else
                {
                    var Data = await _project.GetDetailByIdLastDate(id);
                    return Data != null ? Ok(Data) : NoContent();
                }
                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Add(RProjectForlderDto model)
        {
            try
            {
                var Data = await _project.Create(model);
                return Data == true ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> UpdateState(Guid id)
        {
            try
            {
                var Data = await _project.UpdateState(id);

                return Data == true ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCost(ProjectFolderCostsDto model)
        {
            try
            {
                var Data = await _project.UpdateCost(model);

                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
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
