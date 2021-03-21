using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.API.Business.Interfaces;
using e_Commerce.API.Business.Models;
using e_Commerce.API.Business.Models.Product;
using e_Commerce.API.Common;
using e_Commerce.API.Common.Enums;
using e_Commerce.API.Common.Model;
using e_Commerce.API.Data.Entity;
using e_Commerce.API.Filters;
using e_Commerce.API.Models;
using e_Commerce.API.Models.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProductsController(IProductService productService, IWebHostEnvironment hostingEnvironment)
        {
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
        }
       
        [Route("")]
        [HttpGet]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_LIST)]
        public IActionResult GetAllPaginatedWithDetail([FromQuery] GetAllPaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<ProductWithDetail>>();
            responseModel.DisplayLanguage = displayLanguage;
            var fileHostBaseUrl = GetFileHostBaseUrlFromCurrentRequest(); // şirket resimlerinin tam url adres bilgisi icin kullanilacak

            try
            {
                var searchFilter = new ProductSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_NameTR = requestModel.NameTR;
                searchFilter.Filter_NameEN = requestModel.NameEN;
                searchFilter.Filter_CategoryId = requestModel.CategoryId;
                responseModel.Data = _productService.GetAllPaginatedWithDetailBySearchFilter(searchFilter);

                foreach (var item in responseModel.Data.Items)
                {
                    if (!string.IsNullOrEmpty( item.ImageFilePath))
                    {
                        item.ImageFilePath = fileHostBaseUrl + item.ImageFilePath;
                    }
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("forCustomer")]
        [HttpGet]
        [TokenAuthorizeFilter(AuthCodeStatic.PRODUCT_LISTFORCUSTOMER)]
        public IActionResult GetAllForCustomer([FromQuery] GetAllForCustomerRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<List<ProductWithDetail>>();
            responseModel.DisplayLanguage = displayLanguage;
            var fileHostBaseUrl = GetFileHostBaseUrlFromCurrentRequest(); // şirket resimlerinin tam url adres bilgisi icin kullanilacak

            try
            {
                var searchFilter = new ProductSearchForCustomer();
                searchFilter.Filter_NameTR = requestModel.NameTR;
                searchFilter.Filter_NameEN = requestModel.NameEN;
                searchFilter.Filter_CategoryId = requestModel.CategoryId;
                responseModel.Data = _productService.GetAllForCustomer(searchFilter);

                foreach (var item in responseModel.Data)
                {
                    if (!string.IsNullOrEmpty(item.ImageFilePath))
                    {
                        item.ImageFilePath = fileHostBaseUrl + item.ImageFilePath;
                    }
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{Id}")]
        [HttpGet]
        [TokenAuthorizeFilter]
        public IActionResult GetById(long id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Product>();
            responseModel.DisplayLanguage = displayLanguage;
            var fileHostBaseUrl = GetFileHostBaseUrlFromCurrentRequest(); // şirket resimlerinin tam url adres bilgisi icin kullanilacak

            try
            {
                responseModel.Data = _productService.GetById(id);
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                if (!string.IsNullOrEmpty(responseModel.Data.ImageFilePath))
                {
                    responseModel.Data.ImageFilePath = fileHostBaseUrl + responseModel.Data.ImageFilePath;
                }
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }


        [Route("")]
        [HttpPost]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_ADD)]
        public ApiResponseModel<Data.Entity.Product> Add([FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Product>() { DisplayLanguage = displayLanguage };
            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.Id;

            try
            {
                var record = new Data.Entity.Product();
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                record.DescriptionTR = requestModel.DescriptionTR;
                record.DescriptionEN = requestModel.DescriptionEN;
                if (!String.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    record.ImageFilePath = "ProductFiles/" + requestModel.ImageFilePath;
                }
                record.CategoryId = requestModel.CategoryId;
                record.IsDeleted = false;

                var dbResult = _productService.Add(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor

                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                }
                else
                {
                    //hata oluşursa varsa  resmi silmemiz gerekecek
                    if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProductFiles") + "\\" + requestModel.ImageFilePath;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                //hata oluşursa varsa  resmi silmemiz gerekecek
                if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProductFiles") + "\\" + requestModel.ImageFilePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }
            return responseModel;
        }

        [Route("{id:int}")]
        [HttpPut]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_EDIT)]
        public ApiResponseModel<Data.Entity.Product> Edit(int id, [FromBody] AddRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Product>() { DisplayLanguage = displayLanguage };
            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.Id;
            try
            {
                var broadcast = _productService.GetById(id);
                if (broadcast == null)
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "No Records Found";
                    return responseModel;
                }

                var record = new Data.Entity.Product();
                broadcast.Id = requestModel.Id;
                broadcast.NameTR = requestModel.NameTR;
                broadcast.NameEN = requestModel.NameEN;
                broadcast.DescriptionTR = requestModel.DescriptionTR;
                broadcast.DescriptionEN = requestModel.DescriptionEN;
                if (!String.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    broadcast.ImageFilePath = "ProductFiles/" + requestModel.ImageFilePath;
                }
                else
                {
                    broadcast.ImageFilePath = broadcast.ImageFilePath;
                }
                broadcast.CategoryId = requestModel.CategoryId;

                var dbResult = _productService.Update(broadcast);
                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                }
                else
                {
                    //hata oluşursa varsa  resmi silmemiz gerekecek
                    if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProductFiles") + "\\" + requestModel.ImageFilePath;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                //hata oluşursa varsa  resmi silmemiz gerekecek
                if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProductFiles") + "\\" + requestModel.ImageFilePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }
            return responseModel;
        }

        [Route("{Id}")]
        [HttpDelete]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_DELETE)]
        public IActionResult Delete(int id, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Product>();
            responseModel.DisplayLanguage = displayLanguage;

            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.Id;

            try
            {
                var record = _productService.GetById(id);
                record.IsDeleted = true;
                record.DeletedDateTime = DateTime.Now;
                record.DeletedBy = employeeId;
                var dbResult = _productService.Update(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // 'isDeleted= true' yapılan -> entity bilgisi geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("upload-file")]
        [HttpPost]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_ADD, AuthCodeStatic.PAGE_PRODUCT_EDIT)]
        public ApiResponseModel<string> AddProductFiles([FromForm] List<IFormFile> imageFile, [FromHeader] string displayLanguage)
        {
            string imageFilePath = null;
            var responseModel = new ApiResponseModel<string>() { DisplayLanguage = displayLanguage };

            //sonraki işlemlerde bu kontrol kaldırılabilir.
            if (imageFile != null && imageFile.Count > 1)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "sadece 1 resim ekleyebilirsiniz.";
                return responseModel;
            }

            //resim eklenmiş ise; 
            if (imageFile != null && imageFile.Count > 0)
            {

                long size = imageFile.Sum(f => f.Length);
                try
                {
                    foreach (var item in imageFile)
                    {
                        List<string> allowedExtensions = ConfigHelper.ImageExtension;
                        string fileExtension = Path.GetExtension(item.FileName).ToLower();
                        bool isAllowed = allowedExtensions.Contains(fileExtension);
                        if (isAllowed)
                        {
                            if (item.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ProductFiles");

                                //mac cihazlarda -> image isminde boşluk oldugunda url olarak algılamamaktadır. bu yuzden replace ediyoruz.
                                var newImageName = Guid.NewGuid().ToString() + item.FileName.Replace(" ", "");
                                var fileNameWithPath = string.Concat(filePath, "\\", newImageName);
                                imageFilePath = newImageName;
                                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                            }
                            else
                            {
                                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                                responseModel.ResultStatusMessage = "Picture Format Incompatible";
                                return responseModel;
                            }
                        }
                        else
                        {
                            responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                            responseModel.ResultStatusMessage = "Picture Format Incompatible";
                            return responseModel;
                        }

                    }
                }
                catch
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Images Could Not Be Saved";
                    return responseModel;
                }

            }
            else
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Images Could Not Be Saved";
                return responseModel;
            }
            responseModel.Data = imageFilePath;
            responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
            responseModel.ResultStatusMessage = "Success";
            return responseModel;
        }

        [NonAction]
        private string GetFileHostBaseUrlFromCurrentRequest()
        {
            var result = "";

            result = string.Format("{0}://{1}/", Request.Scheme.ToString(), Request.Host.ToString());

            return result;
        }


    }
}
