using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Filters;
using e_Commerce.API.Models;
using e_Commerce.API.Models.ProfileEmployees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileEmployeesController : ControllerBase
    {
        private readonly IProfileEmployeeService _profileEmployeeService;
        public ProfileEmployeesController(IProfileEmployeeService profileEmployeeService)
        {
            _profileEmployeeService = profileEmployeeService;
        }

        [Route("GetAllEmployeePaginatedWithDetail")]
        [HttpGet]
       // [TokenAuthorizeFilter]
        public ApiResponseModel<PaginatedList<Data.Entity.Employee>> GetAllEmployeePaginatedWithDetail([FromQuery] GetAllEmployeePaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<Data.Entity.Employee>>() { DisplayLanguage = displayLanguage };
            try
            {
                var searchFilter = new Business.Models.ProfileEmployee.ProfileEmployeeSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_ProfileId = requestModel.ProfileId;
                searchFilter.Filter_Employee_Name = requestModel.Employee_Name;
                searchFilter.Filter_Employee_LastName = requestModel.Employee_LastName;
                responseModel.Data = _profileEmployeeService.GetAllEmployeePaginatedWithDetailBySearchFilter(searchFilter);
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

        [Route("GetAllEmployeeWhichIsNotIncludedPaginatedWithDetail")]
        [HttpGet]
      //  [TokenAuthorizeFilter]
        public ApiResponseModel<PaginatedList<Data.Entity.Employee>> GetAllEmployeeWhichIsNotIncludedPaginatedWithDetail([FromQuery] GetAllEmployeeWhichIsNotIncludedPaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<Data.Entity.Employee>>() { DisplayLanguage = displayLanguage };
            try
            {
                var searchFilter = new Business.Models.ProfileEmployee.ProfileEmployeeSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_ProfileId = requestModel.ProfileId;
                searchFilter.Filter_Employee_Name = requestModel.Employee_Name;
                searchFilter.Filter_Employee_LastName = requestModel.Employee_LastName;
                responseModel.Data = _profileEmployeeService.GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(searchFilter);

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

        [Route("GetAllProfileByCurrentUser")]
        [HttpGet]
     //   [TokenAuthorizeFilter]
        public ApiResponseModel<List<Data.Entity.Profile>> GetAllProfileByCurrentUser([FromQuery] GetAllProfileByCurrentUserRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<List<Data.Entity.Profile>>();
            responseModel.DisplayLanguage = displayLanguage;

            if (requestModel.EmployeeId <= 0)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Employee Id Must Be Entered";
                return responseModel;
            }
            try
            {
                responseModel.Data = _profileEmployeeService.GetAllProfileByCurrentUser(requestModel.EmployeeId);
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

        [Route("")]
        [HttpPost]
     //   [TokenAuthorizeFilter]
        public ApiResponseModel<Data.Entity.ProfileEmployee> Add([FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.ProfileEmployee>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = new Data.Entity.ProfileEmployee();
                record.EmployeeId = requestModel.EmployeeId;
                record.ProfileId = requestModel.ProfileId;
                var dbResult = _profileEmployeeService.Add(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
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
     //   [TokenAuthorizeFilter]
        public ApiResponseModel<int> DeleteByProfileIdAndAuthId([FromQuery] DeleteByProfileIdAndEmployeeIdRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<int>() { DisplayLanguage = displayLanguage };
            try
            {
                responseModel.Data = _profileEmployeeService.DeleteByProfileIdAndEmployeeId(requestModel.ProfileId, requestModel.EmployeeId);
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
