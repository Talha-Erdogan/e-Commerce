using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SexController : ControllerBase
    {
        private readonly ISexService _sexService;
        public SexController(ISexService sexService)
        {
            _sexService = sexService;
        }

        [Route("")]
        [HttpGet]
        //[TokenAuthorizeFilter] // sadece oturum acilma kontrolu yapilir
        public IActionResult GetSex([FromHeader] string displayLanguage)
        {
            ApiResponseModel<List<Data.Entity.Sex>> responseModel = new ApiResponseModel<List<Data.Entity.Sex>>() { DisplayLanguage = displayLanguage };
            try
            {
                var sex = _sexService.GetAll();
                responseModel.Data = sex;
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }
    }
}
