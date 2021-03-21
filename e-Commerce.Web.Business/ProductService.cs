using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Common.Enums;
using e_Commerce.Web.Business.Helpers;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Product;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace e_Commerce.Web.Business
{
    public class ProductService : IProductService
    {
        public ApiResponseModel<PaginatedList<ProductWithDetail>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, ProductSearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<ProductWithDetail>> result = new ApiResponseModel<PaginatedList<ProductWithDetail>>()
            {
                Data = new PaginatedList<ProductWithDetail>(new List<ProductWithDetail>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //portal api'den çekme işlemi            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Products?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&CategoryId={4}&NameTR={5}&NameEN={6}",
                  searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection, searchFilter.Filter_CategoryId, searchFilter.Filter_NameTR, searchFilter.Filter_NameEN)).Result;

                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<ProductWithDetail>>>().Result;
            }
            return result;
        }

        public ApiResponseModel<List<ProductWithDetail>> GetAllProductForCustomer(string userToken, string displayLanguage, ProductSearchForCustomer searchFilter)
        {
            ApiResponseModel<List<ProductWithDetail>> result = new ApiResponseModel<List<ProductWithDetail>>()
            {
                Data = new List<ProductWithDetail>()
            };
            //portal api'den çekme işlemi            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Products/forCustomer?&CategoryId={0}&NameTR={1}&NameEN={2}",
                 searchFilter.Filter_CategoryId, searchFilter.Filter_NameTR, searchFilter.Filter_NameEN)).Result;

                result = response.Content.ReadAsJsonAsync<ApiResponseModel<List<ProductWithDetail>>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Product> GetById(string userToken, string displayLanguage, long id)
        {
            ApiResponseModel<Product> result = new ApiResponseModel<Product>();
            // portal api'den çekme işlemi             
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Products/" + id)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Product>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Product> Add(string userToken, string displayLanguage, AddProductWithDetail productWithDetail)
        {
            ApiResponseModel<Product> result = new ApiResponseModel<Product>();

            // ilk olarak image varsa o kayıt edilir.
            ApiResponseModel<string> resultImageAdd = new ApiResponseModel<string>();
            if (productWithDetail.ImageInformation != null && productWithDetail.ImageInformation.Length > 0)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                    httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                    HttpResponseMessage response = httpClient.PostAsImageFiles(string.Format("v1/Products/upload-file"), productWithDetail.ImageInformation).Result;
                    resultImageAdd = response.Content.ReadAsJsonAsync<ApiResponseModel<string>>().Result;
                    if (resultImageAdd.ResultStatusCode != ResultStatusCodeStatic.Success)
                    {
                        result.ResultStatusCode = resultImageAdd.ResultStatusCode;
                        result.ResultStatusMessage = resultImageAdd.ResultStatusMessage;
                        return result;
                    }
                }
            }

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
           
                portalApiRequestModel.NameTR = productWithDetail.NameTR;
                portalApiRequestModel.NameEN = productWithDetail.NameEN;
                portalApiRequestModel.DescriptionTR = productWithDetail.DescriptionTR;
                portalApiRequestModel.DescriptionEN = productWithDetail.DescriptionEN;
                if (productWithDetail.ImageInformation != null && productWithDetail.ImageInformation.Length > 0)
                {
                    portalApiRequestModel.ImageFilePath = resultImageAdd.Data;
                }
                portalApiRequestModel.CategoryId = productWithDetail.CategoryId;

                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/Products"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Product>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Product> Edit(string userToken, string displayLanguage, AddProductWithDetail productWithDetail)
        {
            ApiResponseModel<Product> result = new ApiResponseModel<Product>();

            // ilk olarak image varsa o kayıt edilir.
            ApiResponseModel<string> resultImageAdd = new ApiResponseModel<string>();
            if (productWithDetail.ImageInformation != null && productWithDetail.ImageInformation.Length > 0)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                    httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                    HttpResponseMessage response = httpClient.PostAsImageFiles(string.Format("v1/Products/upload-file"), productWithDetail.ImageInformation).Result;
                    resultImageAdd = response.Content.ReadAsJsonAsync<ApiResponseModel<string>>().Result;
                    if (resultImageAdd.ResultStatusCode != ResultStatusCodeStatic.Success)
                    {
                        result.ResultStatusCode = resultImageAdd.ResultStatusCode;
                        result.ResultStatusMessage = resultImageAdd.ResultStatusMessage;
                        return result;
                    }
                }
            }

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.Id = productWithDetail.Id;
                portalApiRequestModel.NameTR = productWithDetail.NameTR;
                portalApiRequestModel.NameEN = productWithDetail.NameEN;
                portalApiRequestModel.DescriptionTR = productWithDetail.DescriptionTR;
                portalApiRequestModel.DescriptionEN = productWithDetail.DescriptionEN;
                if (productWithDetail.ImageInformation != null && productWithDetail.ImageInformation.Length > 0)
                {
                    portalApiRequestModel.ImageFilePath = resultImageAdd.Data;
                }
                portalApiRequestModel.CategoryId = productWithDetail.CategoryId;
                HttpResponseMessage response = httpClient.PutAsJsonAsync(string.Format("v1/Products/" + portalApiRequestModel.Id), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Product>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Product> Delete(string userToken, string displayLanguage, int productId)
        {
            ApiResponseModel<Product> result = new ApiResponseModel<Product>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                HttpResponseMessage response = httpClient.DeleteAsync(string.Format("v1/Products/" + productId)).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Product>>().Result;
            }
            return result;
        } //end of delete
    }
}
