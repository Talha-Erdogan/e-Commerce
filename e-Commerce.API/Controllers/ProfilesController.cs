using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Filters;
using e_Commerce.API.Models;
using e_Commerce.API.Models.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [Route("")]
        [HttpGet]
        //[TokenAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_LIST)]
        public IActionResult GetAllPaginatedWithDetail([FromQuery] GetAllPaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<Data.Entity.Profile>>() { DisplayLanguage = displayLanguage };
            try
            {
                var searchFilter = new Business.Models.Profile.ProfileSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_Code = requestModel.Code;
                searchFilter.Filter_NameTR = requestModel.NameTR;
                searchFilter.Filter_NameEN = requestModel.NameEN;

                responseModel.Data = _profileService.GetAllPaginatedWithDetailBySearchFilter(searchFilter);

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

        [Route("All")]
        [HttpGet]
       // [TokenAuthorizeFilter]
        public IActionResult GetAll([FromHeader] string displayLanguage)
        {
            ApiResponseModel<List<Data.Entity.Profile>> responseModel = new ApiResponseModel<List<Data.Entity.Profile>>() { DisplayLanguage = displayLanguage };
            try
            {
                var profile = _profileService.GetAll();
                responseModel.Data = profile;
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


        [Route("{id}")]
        [HttpGet]
       // [TokenAuthorizeFilter]
        public IActionResult GetById(int id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Profile>() { DisplayLanguage = displayLanguage };
            try
            {
                responseModel.Data = _profileService.GetById(id);
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

        [HttpPost]
       // [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_ADD)]
        public IActionResult Add([FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Profile>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = new Data.Entity.Profile();
                record.Code = requestModel.Code;
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                record.IsDeleted = false;
                var dbResult = _profileService.Add(record);
                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{Id}")]
        [HttpPut]
       // [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_EDIT)]
        public IActionResult Edit(int id, [FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Profile>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = _profileService.GetById(id);
                if (record == null)
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "No Records Found";
                    return NotFound(responseModel);
                }
                record.Code = requestModel.Code;
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                var dbResult = _profileService.Update(record);
                if (dbResult > 0)
                {
                    responseModel.Data = record;
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{id}")]
        [HttpDelete]
       // [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_DELETE)]
        public IActionResult Delete(int id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Profile>() { DisplayLanguage = displayLanguage };
            try
            {
                var record = _profileService.GetById(id);
                record.IsDeleted = true;
                record.DeletedDateTime = DateTime.Now;
                record.DeletedBy = 1;//todo: tokendan elde edilecek
                var dbResult = _profileService.Update(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // 'isDeleted= true' yapılan -> entity bilgisi geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);

                }
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
