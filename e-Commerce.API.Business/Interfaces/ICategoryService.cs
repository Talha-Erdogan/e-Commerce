using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Category;
using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface ICategoryService
    {
        PaginatedList<Category> GetAllPaginatedWithDetailBySearchFilter(CategorySearchFilter searchFilter);
        List<Category> GetAll();
        Category GetById(int id);
        int Add(Category record);
        int Update(Category record);
    }
}
