using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Helpers;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Profile;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace e_Commerce.Web.Business
{
    public class ProfileService : IProfileService
    {
        //api ile bağlandıgımız servisler
        public ApiResponseModel<PaginatedList<Profile>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProfileSearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<Profile>> result = new ApiResponseModel<PaginatedList<Profile>>()
            {
                Data = new PaginatedList<Profile>(new List<Profile>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //portal api'den çekme işlemi            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Profiles?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&Code={4}&NameTR={5}&NameEN={6}",
                  searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection, searchFilter.Filter_Code, searchFilter.Filter_NameTR, searchFilter.Filter_NameEN)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<Profile>>>().Result;
            }
            return result;
        }

        public ApiResponseModel<List<Profile>> GetAll(string userToken, string displayLanguage)
        {
            ApiResponseModel<List<Profile>> result = new ApiResponseModel<List<Profile>>();
            // portal api'den çekme işlemi 
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Profiles/All")).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<List<Profile>>>().Result;
            }
            return result;
        }
        public ApiResponseModel<Profile> GetById(string userToken, string displayLanguage, int id)
        {
            ApiResponseModel<Profile> result = new ApiResponseModel<Profile>();
            // portal api'den çekme işlemi             
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Profiles/" + id)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Profile>>().Result;
            }
            return result;
        }
        public ApiResponseModel<Profile> Add(string userToken, string displayLanguage, Profile profile)
        {
            ApiResponseModel<Profile> result = new ApiResponseModel<Profile>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.Code = profile.Code;
                portalApiRequestModel.NameTR = profile.NameTR;
                portalApiRequestModel.NameEN = profile.NameEN;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/Profiles"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Profile>>().Result;
            }
            return result;
        }
        public ApiResponseModel<Profile> Edit(string userToken, string displayLanguage, Profile profile)
        {
            ApiResponseModel<Profile> result = new ApiResponseModel<Profile>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.Id = profile.Id;
                portalApiRequestModel.Code = profile.Code;
                portalApiRequestModel.NameTR = profile.NameTR;
                portalApiRequestModel.NameEN = profile.NameEN;
                HttpResponseMessage response = httpClient.PutAsJsonAsync(string.Format("v1/Profiles/" + portalApiRequestModel.Id), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Profile>>().Result;
            }
            return result;
        }
        public ApiResponseModel<Profile> Delete(string userToken, string displayLanguage, int profileId)
        {
            ApiResponseModel<Profile> result = new ApiResponseModel<Profile>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.DeleteAsync(string.Format("v1/Profiles/" + profileId)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Profile>>().Result;
            }
            return result;
        }//end of delete
    }
}
