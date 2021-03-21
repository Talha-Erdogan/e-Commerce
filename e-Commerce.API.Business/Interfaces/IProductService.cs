using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Product;
using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface IProductService
    {
        PaginatedList<ProductWithDetail> GetAllPaginatedWithDetailBySearchFilter(ProductSearchFilter searchFilter);
        List<Product> GetAll();
        List<ProductWithDetail> GetAllForCustomer(ProductSearchForCustomer searchForCustomer);
        Product GetById(long id);
        int Add(Product record);
        int Update(Product record);
    }
}
