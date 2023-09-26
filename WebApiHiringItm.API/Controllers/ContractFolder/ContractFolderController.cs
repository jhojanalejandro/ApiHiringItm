using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using WebApiHiringItm.CORE.Core.ProjectFolders.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Dto.Contrato;
using WebApiHiringItm.MODEL.Dto.ContratoDto;

namespace WebApiHiringItm.API.Controllers.ContractFolder
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ContractFolderController : ControllerBase
    {
        private readonly IProjectFolder _project;

        public ContractFolderController(IProjectFolder proeject)
        {
            _project = proeject;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllContracts(bool inProgress, string tipoModulo)
        {
            try
            {
                List<ContractListDto> projectFolders = new List<ContractListDto>();
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

        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            try
            {
                var Data = await _project.GetAllHistory();
                return Data != null ? Ok(Data) : NoContent();
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
                var Data = await _project.GetById(id);
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetByIdDetailList(Guid id, bool tipoConsulta)
        {
            try
            {
                if (tipoConsulta)
                {
                    var Data = await _project.GetDetailByIdList(id);
                    return Data != null ? Ok(Data) : NoContent();
                }
                else
                {
                    var Data = await _project.GetDetailByIdLastDate(id);
                    return Data != null ? Ok(Data) : NoContent();
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailByIdContract(Guid id, bool tipoConsulta)
        {
            try
            {
                if (tipoConsulta)
                {
                    var Data = await _project.GetDetailByIdContract(id);
                    return Data != null ? Ok(Data) : NoContent();
                }
                else
                {
                    var Data = await _project.GetDetailByIdLastDate(id);
                    return Data != null ? Ok(Data) : NoContent();
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveContract(RProjectForlderDto model)
        {
            try
            {
                var isSuccess = await _project.SaveContract(model);
                if (isSuccess.Success)
                {
                    var response = ApiResponseHelper.CreateResponse(isSuccess);
                    return Ok(response);
                }else
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


        [HttpGet]
        public async Task<IActionResult> UpdateStateContract(string contractId)
        {
            try
            {
                var isSuccess = await _project.UpdateStateContract(contractId);
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
        public async Task<IActionResult> UpdateCost(ProjectFolderCostsDto modelCost)
        {
            try
            {
                var isSuccess = await _project.UpdateCost(modelCost);
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


        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var Data = await _project.Delete(id);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectRegistered()
        {
            try
            {
                var projects = await _project.GetAllProjectsRegistered();

                return projects != null ? Ok(projects) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AssignmentUserContract(List<AssignmentUserDto> assignmentUser)
        {
            try
            {
                var isSuccess = await _project.AssignmentUser(assignmentUser);
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
        public async Task<IActionResult> SaveTermFileContract(TermContractDto modelTermContract)
        {
            try
            {
                var isSuccess = await _project.SaveTermFileContract(modelTermContract);
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
