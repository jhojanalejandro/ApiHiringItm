using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using WebApiHiringItm.CORE.Core.Payroll.Interface;
using WebApiHiringItm.MODEL.Dto;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.API.Controllers.Payroll
{
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Route("[controller]/[action]")]
    public class PayRollController : ControllerBase
    {
        private readonly IPayrollCore _payroll;
        readonly ITokenAcquisition _tokenAcquisition;

        public PayRollController(IPayrollCore payroll, ITokenAcquisition tokenAcquisition)
        {
            _payroll = payroll;
            _tokenAcquisition = tokenAcquisition;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _payroll.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _payroll.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PayrollDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _payroll.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(PayrollDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _payroll.Create(model);

                //Retornamos datos.
                return Data != 0 ? Ok(Data) : NoContent();
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
                var Data = await _payroll.Delete(id);

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
