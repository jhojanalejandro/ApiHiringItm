using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Contractors.Interface;
using WebApiHiringItm.CORE.Core.MessageHandlingCore.Interface;
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
                var Data = await _messageHandling.SendContractorCount(ids);
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}
