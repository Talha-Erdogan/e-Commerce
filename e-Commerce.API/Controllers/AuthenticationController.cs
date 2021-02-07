using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Common;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Models;
using e_Commerce.API.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProfileDetailService _profileDetailService;

        public AuthenticationController(IEmployeeService employeeService, IAuthenticationService authenticationService, IProfileDetailService profileDetailService)
        {
            _employeeService = employeeService;
            _authenticationService = authenticationService;
            _profileDetailService = profileDetailService;
        }

        [Route("Token")]
        [HttpPost]
        public IActionResult Token([FromBody] TokenRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            ApiResponseModel<TokenResponseModel> responseModel = new ApiResponseModel<TokenResponseModel>() { DisplayLanguage = displayLanguage };
            // parameter validations
            if (requestModel == null || string.IsNullOrEmpty(requestModel.UserName) || string.IsNullOrEmpty(requestModel.Password))
            {
                responseModel.Data = null;
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Parametreler boş olmamalıdır.";
                return BadRequest(responseModel);
            }

            var userLoginResponse = _authenticationService.Login(requestModel.UserName, requestModel.Password, displayLanguage);
            if (userLoginResponse.IsValid)
            {
                responseModel.Data = new TokenResponseModel();
                responseModel.Data.Id = userLoginResponse.EmployeeWithDetail.Id;
                responseModel.Data.TRNationalId = userLoginResponse.EmployeeWithDetail.TRNationalId;
                responseModel.Data.Name = userLoginResponse.EmployeeWithDetail.Name;
                responseModel.Data.LastName = userLoginResponse.EmployeeWithDetail.LastName;
                responseModel.Data.MobilePhone = userLoginResponse.EmployeeWithDetail.MobilePhone;
                responseModel.Data.Email = userLoginResponse.EmployeeWithDetail.Email;
                responseModel.Data.SexId = userLoginResponse.EmployeeWithDetail.SexId;
                responseModel.Data.UserName = userLoginResponse.EmployeeWithDetail.UserName;
                responseModel.Data.Password = "-";
                responseModel.Data.TokenExpirePeriod = 60;
                //responseModel.Data.UserToken = Guid.NewGuid().ToString();
                //detail columns
                responseModel.Data.Sex_NameTR = userLoginResponse.EmployeeWithDetail.Sex_NameTR;
                responseModel.Data.Sex_NameEN = userLoginResponse.EmployeeWithDetail.Sex_NameEN;

                // kullanıcının authcode bilgileri elde edilir
                var userAuthCodeListAsString = _profileDetailService.GetAllAuthCodeByEmployeeIdAsConcatenateString(userLoginResponse.EmployeeWithDetail.Id);

                //jwt token eklenmesi işlevi
                responseModel.Data.UserToken = TokenHelper.CreateToken(userLoginResponse.EmployeeWithDetail, userAuthCodeListAsString); //userToken.Token;

                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);
            }
            else
            {
                responseModel.Data = null;
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = userLoginResponse.ErrorMessage;
                return BadRequest(responseModel);
            }
        }
    }
}
