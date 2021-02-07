using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface IAuthenticationService
    {
        ApiResponseModel<LoginResponseModel> Login(string username, string password, string displayLanguage);
    }
}
