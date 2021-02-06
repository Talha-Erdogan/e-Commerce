using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Profile;
using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface IProfileService
    {
        PaginatedList<Profile> GetAllPaginatedWithDetailBySearchFilter(ProfileSearchFilter searchFilter);
        List<Profile> GetAll();
        Profile GetById(int id);
        int Add(Profile record);
        int Update(Profile record);
    }
}
