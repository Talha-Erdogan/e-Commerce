using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Filters;
using e_Commerce.API.Models;
using e_Commerce.API.Models.ProfileDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileDetailsController : ControllerBase
    {
        private readonly IProfileDetailService _profileDetailService;
        public ProfileDetailsController(IProfileDetailService profileDetailService)
        {
            _profileDetailService = profileDetailService;
        }

        [Route("GetAllAuthByCurrentUser")]
        [HttpGet]
        [TokenAuthorizeFilter]
        public IActionResult GetAllAuthByCurrentUser([FromQuery] GetAllByCurrentUserRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<List<Data.Entity.Auth>>();
            responseModel.DisplayLanguage = displayLanguage;

            if (requestModel.EmployeeId <= 0)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "EmployeeId Id Must Be Entered";
                return BadRequest(responseModel);
            }
            try
            {
                responseModel.Data = _profileDetailService.GetAllAuthByCurrentUser(requestModel.EmployeeId);
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

        [Route("GetAllAuthByProfileId")]
        [HttpGet]
        [TokenAuthorizeFilter]
        public IActionResult GetAllAuthByProfileId([FromQuery] GetAllAuthByProfileIdRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<List<Data.Entity.Auth>>() { DisplayLanguage = displayLanguage };
            if (requestModel.ProfileId <= 0)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Profile Id Must Be ntered";
                return BadRequest(responseModel);
            }
            try
            {
                responseModel.Data = _profileDetailService.GetAllAuthByProfileId(requestModel.ProfileId);

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

        [Route("GetAllAuthByProfileIdWhichIsNotIncluded")]
        [HttpGet]
        [TokenAuthorizeFilter]
        public IActionResult GetAllAuthByProfileIdWhichIsNotIncluded([FromQuery] GetAllAuthByProfileIdWhichIsNotIncludedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<List<Data.Entity.Auth>>() { DisplayLanguage = displayLanguage };
            if (requestModel.ProfileId <= 0)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Profile Id Must Be Entered";
                return BadRequest(responseModel);
            }
            try
            {
                responseModel.Data = _profileDetailService.GetAllAuthByProfileIdWhichIsNotIncluded(requestModel.ProfileId);
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

        [Route("")]
        [HttpPost]
        [TokenAuthorizeFilter]
        public ApiResponseModel<Data.Entity.ProfileDetail> Add([FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.ProfileDetail>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = new Data.Entity.ProfileDetail();
                record.AuthId = requestModel.AuthId;
                record.ProfileId = requestModel.ProfileId;

                var dbResult = _profileDetailService.Add(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "CouldNotBeSaved";
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }
            return responseModel;
        }

        [Route("")]
        [HttpDelete]
        [TokenAuthorizeFilter]
        public ApiResponseModel<int> DeleteByProfileIdAndAuthId([FromQuery] DeleteByProfileIdAndAuthIdRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<int>() { DisplayLanguage = displayLanguage };
            try
            {
                responseModel.Data = _profileDetailService.DeleteByProfileIdAndAuthId(requestModel.ProfileId, requestModel.AuthId);
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }
            return responseModel;
        }


    }
}
