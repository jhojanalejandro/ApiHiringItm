using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.ContratoDto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.API.Controllers.MessageHandling
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class MessageHandlingController : ControllerBase
    {
        private readonly IMessageHandlingCore _messageHandling;
        #region BUILDER
        public MessageHandlingController(IMessageHandlingCore messageHandling)
        {
            _messageHandling = messageHandling;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> SendContractorAccount(SendMessageAccountDto ids)
        {
            try
            {
                var isSuccess = await _messageHandling.SendContractorCount(ids);
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
    }
}
