using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Common.Enums;
using e_Commerce.Web.Business.Enums;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models.Category;
using e_Commerce.Web.Filters;
using e_Commerce.Web.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_LIST)]
        public ActionResult List()
        {
            ListViewModel model = new ListViewModel();

            model.Filter = new ListFilterViewModel();
            model.CurrentPage = 1;
            model.PageSize = 10;
            CategorySearchFilter searchFilter = new CategorySearchFilter();
            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;
            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_NameTR = model.Filter.Filter_NameTR;
            searchFilter.Filter_NameEN = model.Filter.Filter_NameEN;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;

            var apiResponseModel = _categoryService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_LIST)]
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

            CategorySearchFilter searchFilter = new CategorySearchFilter();
            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;
            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_NameTR = model.Filter.Filter_NameTR;
            searchFilter.Filter_NameEN = model.Filter.Filter_NameEN;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;

            var apiResponseModel = _categoryService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                model.DataList = apiResponseModel.Data;
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            return View(model);
        }

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_ADD)]
        public ActionResult Add()
        {
            Models.Category.AddViewModel model = new AddViewModel();
            return View(model);
        }

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_ADD)]
        [HttpPost]
        public ActionResult Add(Models.Category.AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Business.Models.Category.Category category = new Business.Models.Category.Category();
            category.NameTR = model.NameTR;
            category.NameEN = model.NameEN;
            var apiResponseModel = _categoryService.Add(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, category);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                return RedirectToAction(nameof(CategoryController.List));
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage != null ? apiResponseModel.ResultStatusMessage : "Kaydedilemedi.";//todo: kulturel olacak NotSaved
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
        }

        [AppAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_EDIT)]
        public ActionResult Edit(int id)
        {
            Models.Category.AddViewModel model = new AddViewModel();
            var apiResponseModel = _categoryService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }

            var category = apiResponseModel.Data;
            if (category == null)
            {
                return View("_ErrorNotExist");
            }

            model.Id = category.Id;
            model.NameTR = category.NameTR;
            model.NameEN = category.NameEN;
            return View(model);
        }

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_CATEGORY_EDIT)]
        [HttpPost]
        public ActionResult Edit(Models.Category.AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var apiResponseModel = _categoryService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.Id);
            if (apiResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }

            var category = apiResponseModel.Data;

            if (category == null)
            {
                return View("_ErrorNotExist");
            }

            category.NameTR = model.NameTR;
            category.NameEN = model.NameEN;

            if (model.SubmitType == "Edit")
            {
                var apiEditResponseModel = _categoryService.Edit(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, category);
                if (apiEditResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
                {
                    ViewBag.ErrorMessage = apiEditResponseModel.ResultStatusMessage != null ? apiEditResponseModel.ResultStatusMessage : "Not Edited";
                    ViewBag.ErrorMessageList = apiEditResponseModel.ErrorMessageList;
                    return View(model);
                }
            }
            if (model.SubmitType == "Delete")
            {
                var apiDeleteResponseModel = _categoryService.Delete(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, category.Id);
                if (apiDeleteResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
                {
                    ViewBag.ErrorMessage = apiDeleteResponseModel.ResultStatusMessage != null ? apiDeleteResponseModel.ResultStatusMessage : "Not Deleted";
                    ViewBag.ErrorMessageList = apiDeleteResponseModel.ErrorMessageList;
                    return View(model);
                }
            }
            return RedirectToAction(nameof(CategoryController.List));
        }
    }
}
