using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface IAuthService
    {
        ApiResponseModel<PaginatedList<Auth>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, AuthSearchFilter searchFilter);
        ApiResponseModel<Auth> GetById(string userToken, string displayLanguage, int id);
        ApiResponseModel<Auth> Add(string userToken, string displayLanguage, Auth auth);
        ApiResponseModel<Auth> Edit(string userToken, string displayLanguage, Auth auth);
        ApiResponseModel<Auth> Delete(string userToken, string displayLanguage, int authId);
    }
}
