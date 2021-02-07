using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Authentication
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
