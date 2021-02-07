using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Helpers;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Employee;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace e_Commerce.Web.Business
{
    public class EmployeeService : IEmployeeService
    {
        public ApiResponseModel<PaginatedList<EmployeeWithDetail>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, EmployeeSearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<EmployeeWithDetail>> result = new ApiResponseModel<PaginatedList<EmployeeWithDetail>>()
            {
                Data = new PaginatedList<EmployeeWithDetail>(new List<EmployeeWithDetail>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //todo: portal api'den çekme işlemi olacak            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                var portalApiRequestModel = new GetAllPaginatedRequestModel();
                portalApiRequestModel.CurrentPage = searchFilter.CurrentPage;
                portalApiRequestModel.PageSize = searchFilter.PageSize;
                portalApiRequestModel.SortOn = searchFilter.SortOn;
                portalApiRequestModel.SortDirection = searchFilter.SortDirection;
                portalApiRequestModel.Name = searchFilter.Filter_Name;
                portalApiRequestModel.LastName = searchFilter.Filter_LastName;

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Employees?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&Name={4}&LastName={5}",
                  searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection, searchFilter.Filter_Name, searchFilter.Filter_LastName)).Result;

                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<EmployeeWithDetail>>>().Result;
            }
            return result;
        }
        public ApiResponseModel<EmployeeWithDetail> GetById(string userToken, string displayLanguage, int id)
        {
            ApiResponseModel<EmployeeWithDetail> result = new ApiResponseModel<EmployeeWithDetail>();
            // portal api'den çekme işlemi             
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Employees/" + id)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<EmployeeWithDetail>>().Result;
            }
            return result;
        }
        public ApiResponseModel<Employee> Add(string userToken, string displayLanguage, Employee employee)
        {
            ApiResponseModel<Employee> result = new ApiResponseModel<Employee>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.TRNationalId = employee.TRNationalId;
                portalApiRequestModel.Name = employee.Name;
                portalApiRequestModel.LastName = employee.LastName;
                portalApiRequestModel.MobilePhone = employee.MobilePhone;
                portalApiRequestModel.Email = employee.Email;
                portalApiRequestModel.SexId = employee.SexId;
                portalApiRequestModel.UserName = employee.UserName;
                portalApiRequestModel.Password = employee.Password;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/Employees"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Employee>>().Result;
            }
            return result;
        }
        public ApiResponseModel<Employee> Edit(string userToken, string displayLanguage, Employee employee)
        {
            ApiResponseModel<Employee> result = new ApiResponseModel<Employee>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.Id = employee.Id;
                portalApiRequestModel.TRNationalId = employee.TRNationalId;
                portalApiRequestModel.Name = employee.Name;
                portalApiRequestModel.LastName = employee.LastName;
                portalApiRequestModel.MobilePhone = employee.MobilePhone;
                portalApiRequestModel.Email = employee.Email;
                portalApiRequestModel.SexId = employee.SexId;
                portalApiRequestModel.UserName = employee.UserName;
                portalApiRequestModel.Password = employee.Password;

                HttpResponseMessage response = httpClient.PutAsJsonAsync(string.Format("v1/Employees/" + portalApiRequestModel.Id), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Employee>>().Result;
            }
            return result;
        }
    }
}
