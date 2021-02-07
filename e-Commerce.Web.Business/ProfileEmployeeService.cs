using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Helpers;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.ProfileEmployee;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace e_Commerce.Web.Business
{
    public class ProfileEmployeeService : IProfileEmployeeService
    {
        //api ile bağlandıgımız servisler
        public ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>> GetAllEmployeePaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProfileEmployeeSearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>> result = new ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>>()
            {
                Data = new PaginatedList<Business.Models.Employee.Employee>(new List<Business.Models.Employee.Employee>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //portal api'den çekme işlemi            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/ProfileEmployees/GetAllEmployeePaginatedWithDetail?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&ProfileId={4}&Employee_Name={5}&Employee_LastName={6}",
                 searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection, searchFilter.Filter_ProfileId, searchFilter.Filter_Employee_Name, searchFilter.Filter_Employee_LastName)).Result;

                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>>>().Result;
            }
            return result;
        }
        public ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>> GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProfileEmployeeSearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>> result = new ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>>()
            {
                Data = new PaginatedList<Business.Models.Employee.Employee>(new List<Business.Models.Employee.Employee>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //portal api'den çekme işlemi            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/ProfileEmployees/GetAllEmployeeWhichIsNotIncludedPaginatedWithDetail?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&ProfileId={4}&Employee_Name={5}&Employee_LastName={6}",
                searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection, searchFilter.Filter_ProfileId, searchFilter.Filter_Employee_Name, searchFilter.Filter_Employee_LastName)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<Business.Models.Employee.Employee>>>().Result;
            }
            return result;
        }
        public ApiResponseModel<List<Business.Models.Profile.Profile>> GetAllProfileByCurrentUser(string userToken, string displayLanguage, int employeeId)
        {
            ApiResponseModel<List<Business.Models.Profile.Profile>> result = new ApiResponseModel<List<Business.Models.Profile.Profile>>();
            //todo: portal api'den çekme işlemi olacak
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/ProfileEmployees/GetAllProfileByCurrentUser?EmployeeId={0}", employeeId)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<List<Business.Models.Profile.Profile>>>().Result;
            }
            return result;
        }
        public ApiResponseModel<ProfileEmployee> Add(string userToken, string displayLanguage, ProfileEmployee profileEmployee)
        {
            ApiResponseModel<ProfileEmployee> result = new ApiResponseModel<ProfileEmployee>();
            // api'yi çağırma yapılır, gelen sonuç elde edilir
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.EmployeeId = profileEmployee.EmployeeId;
                portalApiRequestModel.ProfileId = profileEmployee.ProfileId;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/ProfileEmployees"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<ProfileEmployee>>().Result;
            }
            return result;
        }
        public ApiResponseModel<int> DeleteByProfileIdAndEmployeeId(string userToken, string displayLanguage, int profileId, int employeeId)
        {
            ApiResponseModel<int> result = new ApiResponseModel<int>();
            // api'yi çağırma yapılır, gelen sonuç elde edilir
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.DeleteAsync(string.Format("v1/ProfileEmployees?ProfileId={0}&EmployeeId={1}", profileId, employeeId)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<int>>().Result;
            }
            return result;
        }
    }
}
