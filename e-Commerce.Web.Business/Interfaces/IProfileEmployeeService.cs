using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.ProfileEmployee;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface IProfileEmployeeService
    {
        ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>> GetAllEmployeePaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProfileEmployeeSearchFilter searchFilter);
        ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>> GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProfileEmployeeSearchFilter searchFilter);
        ApiResponseModel<List<Business.Models.Profile.Profile>> GetAllProfileByCurrentUser(string userToken, string displayLanguage, int employeeId);
        ApiResponseModel<ProfileEmployee> Add(string userToken, string displayLanguage, ProfileEmployee profileEmployee);
        ApiResponseModel<int> DeleteByProfileIdAndEmployeeId(string userToken, string displayLanguage, int profileId, int employeeId);
    }
}
