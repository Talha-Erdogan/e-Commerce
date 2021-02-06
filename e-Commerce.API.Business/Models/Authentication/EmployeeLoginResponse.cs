using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Models.Authentication
{
    public class EmployeeLoginResponse
    {
        public bool IsValid { get; set; }
        public Business.Models.Employee.EmployeeWithDetail EmployeeWithDetail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
