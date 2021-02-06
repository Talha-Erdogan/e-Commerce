using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IEmployeeService _employeeService;
        public AuthenticationService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public EmployeeLoginResponse Login(string userName, string password, string language)
        {
            EmployeeLoginResponse result = new EmployeeLoginResponse();

            var employee = _employeeService.GetByUserNameAndPassword(userName, password);
            if (employee != null)
            {
                result.EmployeeWithDetail = employee;
                result.IsValid = true;
                result.ErrorMessage = "";
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = "Kullanıcı veritabanında bulunamadı.";
            }
            return result;
        }
    }
}
