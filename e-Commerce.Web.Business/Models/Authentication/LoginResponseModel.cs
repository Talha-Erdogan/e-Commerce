using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Authentication
{
    public class LoginResponseModel : Employee.EmployeeWithDetail
    {
        public string UserToken { get; set; }
        public int TokenExpirePeriod { get; set; }
    }
}
