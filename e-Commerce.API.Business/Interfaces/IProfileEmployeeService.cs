using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.ProfileEmployee;
using e_Commerce.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Interfaces
{
    public interface IProfileEmployeeService
    {
        PaginatedList<Employee> GetAllEmployeePaginatedWithDetailBySearchFilter(ProfileEmployeeSearchFilter searchFilter);
        PaginatedList<Employee> GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(ProfileEmployeeSearchFilter searchFilter);
        List<Profile> GetAllProfileByCurrentUser(int employeeId);
        List<Profile> GetAllProfileByEmployeeIdAndAuthCode(int employeeId, string authCode);
        int Add(ProfileEmployee record);
        int DeleteByProfileIdAndEmployeeId(int profileId, int employeeId);
    }
}
