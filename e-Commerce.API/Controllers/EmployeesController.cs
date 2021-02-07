using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Data.Entity;
using e_Commerce.API.Filters;
using e_Commerce.API.Models;
using e_Commerce.API.Models.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Route("")]
        [HttpGet]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_EMPLOYEE_LIST)]
        public IActionResult GetAllPaginatedWithDetail([FromQuery] GetAllPaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<Business.Models.Employee.EmployeeWithDetail>>() { DisplayLanguage = displayLanguage };
            try
            {
                var searchFilter = new Business.Models.Employee.EmployeeSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_Name = requestModel.Name;
                searchFilter.Filter_LastName = requestModel.LastName;
                responseModel.Data = _employeeService.GetAllPaginatedWithDetailBySearchFilter(searchFilter);

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
        [TokenAuthorizeFilter]
        public IActionResult GetById(int id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Business.Models.Employee.EmployeeWithDetail>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                responseModel.Data = _employeeService.GetByIdWithDetail(id);
                if (responseModel.Data == null)
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "NoRecordsFound";
                    return NotFound(responseModel);
                }
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
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_EMPLOYEE_ADD)]
        public IActionResult Add([FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Employee>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = new Employee();
                record.TRNationalId = requestModel.TRNationalId;
                record.Name = requestModel.Name;
                record.LastName = requestModel.LastName;
                record.MobilePhone = requestModel.MobilePhone;
                record.Email = requestModel.Email;
                record.SexId = requestModel.SexId;
                record.UserName = requestModel.UserName;
                record.Password = requestModel.Password;
                var dbResult = _employeeService.Add(record);

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
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_EMPLOYEE_EDIT)]
        public IActionResult Edit(int id, [FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Employee>() { DisplayLanguage = displayLanguage };
            try
            {
                var record = _employeeService.GetById(id);
                record.TRNationalId = requestModel.TRNationalId;
                record.Name = requestModel.Name;
                record.LastName = requestModel.LastName;
                record.MobilePhone = requestModel.MobilePhone;
                record.Email = requestModel.Email;
                record.SexId = requestModel.SexId;
                record.UserName = requestModel.UserName;
                record.Password = requestModel.Password;
                var dbResult = _employeeService.Update(record);
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
    }
}
