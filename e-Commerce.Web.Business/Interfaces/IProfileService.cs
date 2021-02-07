using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface IProfileService
    {
        ApiResponseModel<PaginatedList<Profile>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProfileSearchFilter searchFilter);
        ApiResponseModel<List<Profile>> GetAll(string userToken, string displayLanguage);
        ApiResponseModel<Profile> GetById(string userToken, string displayLanguage, int id);
        ApiResponseModel<Profile> Add(string userToken, string displayLanguage, Profile profile);
        ApiResponseModel<Profile> Edit(string userToken, string displayLanguage, Profile profile);
        ApiResponseModel<Profile> Delete(string userToken, string displayLanguage, int profileId);
    }
}
