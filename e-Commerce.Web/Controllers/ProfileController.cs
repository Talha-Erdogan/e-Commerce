using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Common.Enums;
using e_Commerce.Web.Business.Enums;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models.Profile;
using e_Commerce.Web.Filters;
using e_Commerce.Web.Models.Profile;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_LIST)]
        public ActionResult List()
        {
            ListViewModel model = new ListViewModel();
            model.Filter = new ListFilterViewModel();
            model.CurrentPage = 1;
            model.PageSize = 10;
            ProfileSearchFilter searchFilter = new ProfileSearchFilter();
            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;
            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_Code = model.Filter.Filter_Code;
            searchFilter.Filter_NameTR = model.Filter.Filter_NameTR;
            searchFilter.Filter_NameEN = model.Filter.Filter_NameEN;
            var apiResponseModel = _profileService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
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

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_LIST)]
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

            ProfileSearchFilter searchFilter = new ProfileSearchFilter();
            searchFilter.CurrentPage = model.CurrentPage.HasValue ? model.CurrentPage.Value : 1;
            searchFilter.PageSize = model.PageSize.HasValue ? model.PageSize.Value : 10;
            searchFilter.SortOn = model.SortOn;
            searchFilter.SortDirection = model.SortDirection;
            searchFilter.Filter_Code = model.Filter.Filter_Code;
            searchFilter.Filter_NameTR = model.Filter.Filter_NameTR;
            searchFilter.Filter_NameEN = model.Filter.Filter_NameEN;
            model.CurrentLanguageTwoChar = SessionHelper.CurrentLanguageTwoChar;
            var apiResponseModel = _profileService.GetAllPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, searchFilter);
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

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_ADD)]
        public ActionResult Add()
        {
            Models.Profile.AddViewModel model = new AddViewModel();
            return View(model);
        }

        //[AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_ADD)]
        [HttpPost]
        public ActionResult Add(Models.Profile.AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Business.Models.Profile.Profile profile = new Business.Models.Profile.Profile();
            profile.Code = model.Code;
            profile.NameTR = model.NameTR;
            profile.NameEN = model.NameEN;
            var apiResponseModel = _profileService.Add(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profile);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                return RedirectToAction(nameof(ProfileController.List));
            }
            else
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage != null ? apiResponseModel.ResultStatusMessage : "Not Saved.";
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
        }


       // [AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_EDIT)]
        public ActionResult Edit(int id)
        {
            Models.Profile.AddViewModel model = new AddViewModel();
            var apiResponseModel = _profileService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, id);
            if (apiResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            var profile = apiResponseModel.Data;
            if (profile == null)
            {
                return View("_ErrorNotExist");
            }
            model.Id = profile.Id;
            model.Code = profile.Code;
            model.NameTR = profile.NameTR;
            model.NameEN = profile.NameEN;
            return View(model);
        }

       // [AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILE_EDIT)]
        [HttpPost]
        public ActionResult Edit(Models.Profile.AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var apiResponseModel = _profileService.GetById(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.Id);
            if (apiResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                ViewBag.ErrorMessage = apiResponseModel.ResultStatusMessage;
                ViewBag.ErrorMessageList = apiResponseModel.ErrorMessageList;
                return View(model);
            }
            var profile = apiResponseModel.Data;
            if (profile == null)
            {
                return View("_ErrorNotExist");
            }
            profile.Code = model.Code;
            profile.NameTR = model.NameTR;
            profile.NameEN = model.NameEN;
            if (model.SubmitType == "Edit")
            {
                var apiEditResponseModel = _profileService.Edit(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profile);
                if (apiEditResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
                {
                    ViewBag.ErrorMessage = apiEditResponseModel.ResultStatusMessage != null ? apiEditResponseModel.ResultStatusMessage : "Not Edited";
                    ViewBag.ErrorMessageList = apiEditResponseModel.ErrorMessageList;
                    return View(model);
                }
            }
            if (model.SubmitType == "Delete")
            {
                var apiDeleteResponseModel = _profileService.Delete(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profile.Id);
                if (apiDeleteResponseModel.ResultStatusCode != ResultStatusCodeStatic.Success)
                {
                    ViewBag.ErrorMessage = apiDeleteResponseModel.ResultStatusMessage != null ? apiDeleteResponseModel.ResultStatusMessage : "Not Deleted";
                    ViewBag.ErrorMessageList = apiDeleteResponseModel.ErrorMessageList;
                    return View(model);
                }
            }
            return RedirectToAction(nameof(ProfileController.List));
        }

    }
}
