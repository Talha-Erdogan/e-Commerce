using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Category;
using e_Commerce.API.Common;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Common.Model;
using e_Commerce.API.Data.Entity;
using e_Commerce.API.Filters;
using e_Commerce.API.Models;
using e_Commerce.API.Models.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Route("withDetail")]
        [HttpGet]
        //[TokenAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_LIST)]
        public IActionResult GetAllPaginatedWithDetail([FromQuery] GetAllPaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<Category>>();
            responseModel.DisplayLanguage = displayLanguage;

            try
            {
                var searchFilter = new CategorySearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_NameTR = requestModel.NameTR;
                searchFilter.Filter_NameEN = requestModel.NameEN;
                responseModel.Data = _categoryService.GetAllPaginatedWithDetailBySearchFilter(searchFilter);

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

        public IActionResult GetAll([FromHeader] string displayLanguage)
        {
            ApiResponseModel<List<Data.Entity.Category>> responseModel = new ApiResponseModel<List<Data.Entity.Category>>() { DisplayLanguage = displayLanguage };
            try
            {
                var categories = _categoryService.GetAll();
                responseModel.Data = categories;
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


        [Route("{Id}")]
        [HttpGet]
       // [TokenAuthorizeFilter]
        public IActionResult GetById(int id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Category>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                responseModel.Data = _categoryService.GetById(id);
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
        //[TokenAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_ADD)]
        public IActionResult Add([FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Category>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = new Category();
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                record.IsDeleted = false;

                var dbResult = _categoryService.Add(record);

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
       //[TokenAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_EDIT)]
        public IActionResult Edit(int id, [FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Category>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = _categoryService.GetById(id);
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                var dbResult = _categoryService.Update(record);
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

        [Route("{Id}")]
        [HttpDelete]
        //[TokenAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_DELETE)]
        public IActionResult Delete(int id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Category>();
            responseModel.DisplayLanguage = displayLanguage;

            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.Id;

            try
            {
                var record = _categoryService.GetById(id);
                record.IsDeleted = true;
                record.DeletedDateTime = DateTime.Now;
                record.DeletedBy = employeeId;
                var dbResult = _categoryService.Update(record);

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
