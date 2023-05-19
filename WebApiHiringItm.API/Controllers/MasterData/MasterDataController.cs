﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiHiringItm.CORE.Core.FileCore.Interface;
using WebApiHiringItm.CORE.Core.MasterDataCore.Interface;
using WebApiHiringItm.MODEL.Entities;

namespace WebApiHiringItm.API.Controllers.MasterData
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataCore _masterDataCore;

        public MasterDataController(IMasterDataCore masterDataCore)
        {
            _masterDataCore = masterDataCore;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMinutes()
        {
            try
            {
                var Data = await _masterDataCore.GetAllMinutesType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFileTypes()
        {
            try
            {
                var Data = await _masterDataCore.GetAllFileType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCpcType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllCpcType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusContract()
        {
            try
            {
                var Data = await _masterDataCore.GetSatatusContract();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllElementType()
        {
            try
            {
                var Data = await _masterDataCore.GetAllElementType();
                return Data != null ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }
    }
}