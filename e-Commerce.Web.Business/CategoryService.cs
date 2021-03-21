using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Helpers;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Category;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace e_Commerce.Web.Business
{
    public class CategoryService : ICategoryService
    {
        //api ile bağlandıgımız servisler
        public ApiResponseModel<PaginatedList<Category>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, CategorySearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<Category>> result = new ApiResponseModel<PaginatedList<Category>>()
            {
                Data = new PaginatedList<Category>(new List<Category>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //portal api'den çekme işlemi            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Categories/withDetail?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&&NameTR={4}&NameEN={5}",
                  searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection,  searchFilter.Filter_NameTR, searchFilter.Filter_NameEN)).Result;

                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<Category>>>().Result;
            }
            return result;
        }

        public ApiResponseModel<List<Category>> GetAll(string userToken, string displayLanguage)
        {
            ApiResponseModel<List<Category>> result = new ApiResponseModel<List<Category>>();
            // portal api'den çekme işlemi 
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Categories")).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<List<Category>>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Category> GetById(string userToken, string displayLanguage, int id)
        {
            ApiResponseModel<Category> result = new ApiResponseModel<Category>();
            // portal api'den çekme işlemi             
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Categories/" + id)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Category>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Category> Add(string userToken, string displayLanguage, Category category)
        {
            ApiResponseModel<Category> result = new ApiResponseModel<Category>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.NameTR = category.NameTR;
                portalApiRequestModel.NameEN = category.NameEN;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/Categories"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Category>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Category> Edit(string userToken, string displayLanguage, Category category)
        {
            ApiResponseModel<Category> result = new ApiResponseModel<Category>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.Id = category.Id;
                portalApiRequestModel.NameTR = category.NameTR;
                portalApiRequestModel.NameEN = category.NameEN;
                HttpResponseMessage response = httpClient.PutAsJsonAsync(string.Format("v1/Categories/" + portalApiRequestModel.Id), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Category>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Category> Delete(string userToken, string displayLanguage, int categoryId)
        {
            ApiResponseModel<Category> result = new ApiResponseModel<Category>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.DeleteAsync(string.Format("v1/Categories/" + categoryId)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Category>>().Result;
            }
            return result;
        } //end of delete
    }
}
