using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface IProductService
    {
        ApiResponseModel<PaginatedList<Product>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProductSearchFilter searchFilter);
        ApiResponseModel<Product> GetById(string userToken, string displayLanguage, int id);
        ApiResponseModel<Product> Add(string userToken, string displayLanguage, AddProductWithDetail productWithDetail);
        ApiResponseModel<Product> Edit(string userToken, string displayLanguage, AddProductWithDetail productWithDetail);
        ApiResponseModel<Product> Delete(string userToken, string displayLanguage, int productId);
    }
}
