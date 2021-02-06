using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Auth;
using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface IAuthService
    {
        PaginatedList<Auth> GetAllPaginatedWithDetailBySearchFilter(AuthSearchFilter searchFilter);
        List<Auth> GetAll();
        Auth GetById(int id);
        int Add(Auth record);
        int Update(Auth record);
    }
}
