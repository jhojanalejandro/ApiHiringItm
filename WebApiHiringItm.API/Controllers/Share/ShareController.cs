using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.Share.Interface;
using WebApiHiringItm.CORE.Core.User.Interface;
using WebApiHiringItm.CORE.Helpers.GenericResponse;
using WebApiHiringItm.MODEL.Dto.Share;
using WebApiHiringItm.MODEL.Dto.Usuario;

namespace WebApiHiringItm.API.Controllers.Share
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShareController : ControllerBase
    {
        private readonly IGenericCore _generic;

        public ShareController(IGenericCore generic)
        {
            _generic = generic;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSessionPanel(SessionPanelDto model)
        {
            try
            {
                var isSuccess = await _generic.UpdateSessionPanel(model);
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
