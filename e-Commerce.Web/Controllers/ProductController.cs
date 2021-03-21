using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Common.Enums;
using e_Commerce.Web.Business.Enums;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Product;
using e_Commerce.Web.Filters;
using e_Commerce.Web.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_Commerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_LIST)]
        public ActionResult List(string errorMessage = "")
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }
            ListViewModel model = new ListViewModel();
            model.Filter = new ListFilterViewModel();
            model.CurrentPage = 1;
            model.PageSize = 10;

            ProductSearchFilter searchFilter = new ProductSearchFilter();
            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;
            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_NameTR = model.Filter.NameTR;
            searchFilter.Filter_NameEN = model.Filter.NameEN;
            searchFilter.Filter_CategoryId = model.Filter.CategoryId;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            // select lists
            model.FilterCategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);

            var apiResponseModel = _productService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                model.DataList = new Business.Models.PaginatedList<ProductWithDetail>();
                model.DataList.Items = new List<ProductWithDetail>();
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_LIST)]
        [HttpPost]
        public ActionResult List(ListViewModel model)
        {
            // filter bilgilerinin default boş değerlerle doldurulması sağlanıyor
            if (model.Filter == null)
            {
                model.Filter = new ListFilterViewModel();
            }

            if (!model.CurrentPage.HasValue)
            {
                model.CurrentPage = 1;
            }

            if (!model.PageSize.HasValue)
            {
                model.PageSize = 10;
            }

            ProductSearchFilter searchFilter = new ProductSearchFilter();

            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;

            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_NameTR = model.Filter.NameTR;
            searchFilter.Filter_NameEN = model.Filter.NameEN;
            searchFilter.Filter_CategoryId = model.Filter.CategoryId;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            // select lists
            model.FilterCategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);


            var apiResponseModel = _productService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                model.DataList = new Business.Models.PaginatedList<ProductWithDetail>();
                model.DataList.Items = new List<ProductWithDetail>();
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_ADD)]
        public ActionResult Add()
        {
            Models.Product.AddViewModel model = new AddViewModel();

            //select lists
            model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
            model.SessionGuid = Guid.NewGuid().ToString();
            model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_ADD)]
        [HttpPost]
        public ActionResult Add(Models.Product.AddViewModel model, IFormFile ImageFilePath)
        {
            //image ekleme için faydalanılan link -> https://www.webtrainingroom.com/aspnetcore/file-upload
            model.ImageFilePath = ImageFilePath;
            if (ImageFilePath != null)
            {
                string imageFileName = ImageFilePath.FileName;
                model.ImageInformation = ConvertIFormFileToImageInformation(ImageFilePath);
                SessionHelper.SetObject(model.SessionGuid, model.ImageInformation);

            }
            var sessionImage = SessionHelper.GetObject<ImageInformation>(model.SessionGuid);
            if (sessionImage != null)
            {
                model.SessionImageFileName = sessionImage.FileName;
                if (ImageFilePath == null)
                {
                    model.ImageInformation = sessionImage;
                }
            }

            if (!ModelState.IsValid)
            {
                //select lists
                model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                return View(model);
            }

            //başlıklardan en az birinin girilmesi istenmektedir. Bunun kontrolu işlevidir.
            if (string.IsNullOrEmpty(model.NameTR) && string.IsNullOrEmpty(model.NameEN))
            {
                model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                ViewBag.ErrorMessage = "Name TR Or Name EN Is Required";
                return View(model);
            }


            Business.Models.Product.AddProductWithDetail productWithDetail = new Business.Models.Product.AddProductWithDetail();
            productWithDetail.NameTR = model.NameTR;
            productWithDetail.NameEN = model.NameEN;
            productWithDetail.CategoryId = model.CategoryId;
            productWithDetail.DescriptionTR = model.DescriptionTR;
            productWithDetail.DescriptionEN = model.DescriptionEN;
            productWithDetail.IsDeleted = false;
            productWithDetail.ImageInformation = model.ImageInformation;

            var apiResponseModel = _productService.Add(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, productWithDetail);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                SessionHelper.SetObject(model.SessionGuid, null);
                if (SessionHelper.CheckAuthForCurrentUser(AuthCodeStatic.PAGE_PRODUCT_LIST))
                {
                    return RedirectToAction(nameof(ProductController.List), "Product");
                }
                return RedirectToAction(nameof(HomeController.Index), "Home"); //todo: şimdilik bu sekilde ayarlandı.
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage != null ? apiResponseModel.ResultStatusMessage : "Kaydedilemedi.";//todo: kulturel olacak NotSaved
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                // todo: select lists
                model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                return View(model);
            }
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_EDIT)]
        public ActionResult Edit(int id)
        {
            Models.Product.AddViewModel model = new AddViewModel();

            var apiProductResponseModel = _productService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiProductResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return RedirectToAction(nameof(ProductController.List), "Product", new { errorMessage = apiProductResponseModel.ResultStatusMessage });
            }
            if (apiProductResponseModel == null || apiProductResponseModel.Data == null || apiProductResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return View("_ErrorNotExist");
            }

            model.SessionGuid = Guid.NewGuid().ToString();
            model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
            model.Id = apiProductResponseModel.Data.Id;
            model.CategoryId = apiProductResponseModel.Data.CategoryId;
            model.SessionImageFileName = String.IsNullOrEmpty(apiProductResponseModel.Data.ImageFilePath) ? "" : ConfigHelper.ApiBaseUrl + apiProductResponseModel.Data.ImageFilePath;
            model.NameTR = apiProductResponseModel.Data.NameTR;
            model.NameEN = apiProductResponseModel.Data.NameEN;
            model.CategoryId = apiProductResponseModel.Data.CategoryId;
            model.DescriptionTR = apiProductResponseModel.Data.DescriptionTR;
            model.DescriptionEN = apiProductResponseModel.Data.DescriptionEN;

            //select list
            model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_EDIT)]
        [HttpPost]
        public ActionResult Edit(Models.Product.AddViewModel model, IFormFile ImageFilePath)
        {
            //image ekleme için faydalanılan link -> https://www.webtrainingroom.com/aspnetcore/file-upload
            model.ImageFilePath = ImageFilePath;
            if (ImageFilePath != null)
            {
                string imageFileName = ImageFilePath.FileName;
                model.ImageInformation = ConvertIFormFileToImageInformation(ImageFilePath);
                SessionHelper.SetObject(model.SessionGuid, model.ImageInformation);
            }
            var sessionImage = SessionHelper.GetObject<ImageInformation>(model.SessionGuid);
            if (sessionImage != null)
            {
                model.SessionImageFileName = sessionImage.FileName;
                if (ImageFilePath == null)
                {
                    model.ImageInformation = sessionImage;
                }
            }

            if (!ModelState.IsValid)
            {
                //select lists
                model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                return View(model);
            }

            //başlıklardan en az birinin girilmesi istenmektedir. Bunun kontrolu işlevidir.
            if (string.IsNullOrEmpty(model.NameTR) && string.IsNullOrEmpty(model.NameEN))
            {
                model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                model.DisplayLanguage = SessionHelper.CurrentLanguageTwoChar;
                ViewBag.ErrorMessage = "Name TR Or Name EN Is Required";
                return View(model);
            }

            var apiProductResponseModel = _productService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.Id);
            if (apiProductResponseModel == null || apiProductResponseModel.Data == null || apiProductResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return View("_ErrorNotExist");
            }

            Business.Models.Product.AddProductWithDetail productWithDetail = new Business.Models.Product.AddProductWithDetail();
            productWithDetail.Id = apiProductResponseModel.Data.Id;
            productWithDetail.CategoryId = model.CategoryId;
            productWithDetail.NameEN = model.NameEN;
            productWithDetail.NameTR = model.NameTR;
            productWithDetail.DescriptionTR = model.DescriptionTR;
            productWithDetail.DescriptionEN = model.DescriptionEN;
            productWithDetail.ImageInformation = model.ImageInformation;

            var apiResponseModel = _productService.Edit(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, productWithDetail);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                SessionHelper.SetObject(model.SessionGuid, null);
                if (SessionHelper.CheckAuthForCurrentUser(AuthCodeStatic.PAGE_PRODUCT_LIST))
                {
                    return RedirectToAction(nameof(ProductController.List), "Product");
                }
                return RedirectToAction(nameof(HomeController.Index), "Home"); //todo: şimdilik bu sekilde ayarlandı.
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage != null ? apiResponseModel.ResultStatusMessage : "Kaydedilemedi.";//todo: kulturel olacak NotSaved
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                // todo: select lists
                model.CategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
                return View(model);
            }
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PRODUCT_DISPLAY)]
        public ActionResult Display(int id)
        {
            var apiBroadcastResponseModel = _productService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiBroadcastResponseModel == null || apiBroadcastResponseModel.Data == null || apiBroadcastResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return View("_ErrorNotExist");
            }

            e_Commerce.Web.Business.Models.Product.Product model = new e_Commerce.Web.Business.Models.Product.Product();
            model = apiBroadcastResponseModel.Data;
            return View(model);
        }



        [NonAction]
        private ImageInformation ConvertIFormFileToImageInformation(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return null;
            }
            ImageInformation imageInformation = new ImageInformation();
            byte[] imageByteData;
            using (var br = new BinaryReader(imageFile.OpenReadStream()))
            {
                imageByteData = br.ReadBytes((int)imageFile.OpenReadStream().Length);
            }

            imageInformation.ImageByteData = imageByteData;
            imageInformation.FileName = imageFile.FileName;
            imageInformation.Length = imageFile.Length;
            return imageInformation;
        }

        [NonAction]
        private List<SelectListItem> GetCategorySelectList(string userToken, string displayLanguage)
        {
            // kategori tipi kayıtları listelenir
            List<SelectListItem> resultList = new List<SelectListItem>();

            // api'den çekim yapılacak
            var apiResponseModel = _categoryService.GetAll(userToken, displayLanguage);
            resultList = apiResponseModel.Data.OrderBy(r => displayLanguage == "tr" ? r.NameTR : r.NameEN).Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = displayLanguage == "tr" ? r.NameTR : r.NameEN }).ToList();
            return resultList;
        }






        //müşteri ekranı için
        [AppAuthorizeFilter(AuthCodeStatic.PRODUCT_LISTFORCUSTOMER)]
        public ActionResult ListForCustomer(string errorMessage = "")
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }
            ListForCustomerViewModel model = new ListForCustomerViewModel();

            model.Filter = new ProductSearchForCustomer();
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            // select lists
            model.FilterCategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);

            var apiResponseModel = _productService.GetAllProductForCustomer(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.Filter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                model.DataList = new List<ProductWithDetail>();
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        [AppAuthorizeFilter(AuthCodeStatic.PRODUCT_LISTFORCUSTOMER)]
        [HttpPost]
        public ActionResult ListForCustomer(ListForCustomerViewModel model)
        {
            // filter bilgilerinin default boş değerlerle doldurulması sağlanıyor
            if (model.Filter == null)
            {
                model.Filter = new ProductSearchForCustomer();
            }
           
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            // select lists
            model.FilterCategorySelectList = GetCategorySelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);

            var apiResponseModel = _productService.GetAllProductForCustomer(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.Filter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                model.DataList = new List<ProductWithDetail>();
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }
    }
}
