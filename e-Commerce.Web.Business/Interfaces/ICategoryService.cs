using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface ICategoryService
    {
        ApiResponseModel<PaginatedList<Category>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, CategorySearchFilter searchFilter);
        ApiResponseModel<Category> GetById(string userToken, string displayLanguage, int id);
        ApiResponseModel<Category> Add(string userToken, string displayLanguage, Category category);
        ApiResponseModel<Category> Edit(string userToken, string displayLanguage, Category category);
        ApiResponseModel<Category> Delete(string userToken, string displayLanguage, int categoryId);
    }
}
