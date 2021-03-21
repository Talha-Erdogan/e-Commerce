using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_Commerce.Web.Business.Common;
using e_Commerce.Web.Business.Common.Enums;
using e_Commerce.Web.Business.Enums;
using e_Commerce.Web.Business.Interfaces;
using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.ProfileEmployee;
using e_Commerce.Web.Filters;
using e_Commerce.Web.Models.ProfileEmployee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_Commerce.Web.Controllers
{
    public class ProfileEmployeeController : Controller
    {
        private readonly IProfileEmployeeService _profileEmployeeService;
        private readonly IProfileService _profileService;
        private readonly IEmployeeService _employeeService;

        public ProfileEmployeeController(IProfileEmployeeService profileEmployeeService, IProfileService profileService, IEmployeeService employeeService)
        {
            _profileEmployeeService = profileEmployeeService;
            _profileService = profileService;
            _employeeService = employeeService;
        }


        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILEEMPLOYEE_BATCHEDIT)]
        public ActionResult BatchEdit()
        {
            BatchEditViewModel model = new BatchEditViewModel();
            model.ProfileSelectList = GetProfileSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
            model.EmployeeList = new DefinedEmployeeListViewModel();
            model.EmployeeList.Filter = new DefinedEmployeeListFilterViewModel();
            model.EmployeeList.DataList = new Business.Models.PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
            model.EmployeeList.CurrentPage = 1;
            model.EmployeeList.PageSize = 10;


            model.EmployeeWhichIsNotIncludeList = new UndefinedEmployeeListViewModel();
            model.EmployeeWhichIsNotIncludeList.Filter = new UndefinedEmployeeListFilterViewModel();
            model.EmployeeWhichIsNotIncludeList.DataList = new Business.Models.PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
            model.EmployeeWhichIsNotIncludeList.CurrentPage = 1;
            model.EmployeeWhichIsNotIncludeList.PageSize = 10;

            return View(model);
        }

        [HttpPost]
        [AppAuthorizeFilter(AuthCodeStatic.PAGE_PROFILEEMPLOYEE_BATCHEDIT)]
        public ActionResult BatchEdit(BatchEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ProfileSelectList = GetProfileSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);

                if (model.ProfileId.HasValue)
                {
                    if (model.EmployeeList.Filter == null)
                    {
                        model.EmployeeList.Filter = new DefinedEmployeeListFilterViewModel();
                    }
                    if (model.EmployeeWhichIsNotIncludeList.Filter == null)
                    {
                        model.EmployeeWhichIsNotIncludeList.Filter = new UndefinedEmployeeListFilterViewModel();
                    }

                    if (!model.EmployeeList.CurrentPage.HasValue)
                    {
                        model.EmployeeList.CurrentPage = 1;
                    }

                    if (!model.EmployeeList.PageSize.HasValue)
                    {
                        model.EmployeeList.PageSize = 10;
                    }

                    if (!model.EmployeeWhichIsNotIncludeList.CurrentPage.HasValue)
                    {
                        model.EmployeeWhichIsNotIncludeList.CurrentPage = 1;
                    }

                    if (!model.EmployeeWhichIsNotIncludeList.PageSize.HasValue)
                    {
                        model.EmployeeWhichIsNotIncludeList.PageSize = 10;
                    }

                    ProfileEmployeeSearchFilter profileEmployeeSearchFilter = new ProfileEmployeeSearchFilter();
                    profileEmployeeSearchFilter.Filter_ProfileId = model.ProfileId.Value;
                    profileEmployeeSearchFilter.CurrentPage = model.EmployeeList.CurrentPage.HasValue ? model.EmployeeList.CurrentPage.Value : 1;
                    profileEmployeeSearchFilter.PageSize = model.EmployeeList.PageSize.HasValue ? model.EmployeeList.PageSize.Value : 10;
                    profileEmployeeSearchFilter.SortOn = model.EmployeeList.SortOn;
                    profileEmployeeSearchFilter.SortDirection = model.EmployeeList.SortDirection;
                    profileEmployeeSearchFilter.Filter_Employee_Name = model.EmployeeList.Filter.Filter_Employee_Name;
                    profileEmployeeSearchFilter.Filter_Employee_LastName = model.EmployeeList.Filter.Filter_Employee_LastName;

                    ProfileEmployeeSearchFilter profileEmployeeWhichIsNotIncludeListSearchFilter = new ProfileEmployeeSearchFilter();
                    profileEmployeeWhichIsNotIncludeListSearchFilter.Filter_ProfileId = model.ProfileId.Value;
                    profileEmployeeWhichIsNotIncludeListSearchFilter.CurrentPage = model.EmployeeWhichIsNotIncludeList.CurrentPage.HasValue ? model.EmployeeWhichIsNotIncludeList.CurrentPage.Value : 1;
                    profileEmployeeWhichIsNotIncludeListSearchFilter.PageSize = model.EmployeeWhichIsNotIncludeList.PageSize.HasValue ? model.EmployeeWhichIsNotIncludeList.PageSize.Value : 10;
                    profileEmployeeWhichIsNotIncludeListSearchFilter.SortOn = model.EmployeeWhichIsNotIncludeList.SortOn;
                    profileEmployeeWhichIsNotIncludeListSearchFilter.SortDirection = model.EmployeeWhichIsNotIncludeList.SortDirection;
                    profileEmployeeWhichIsNotIncludeListSearchFilter.Filter_Employee_Name = model.EmployeeWhichIsNotIncludeList.Filter.Filter_Employee_Name;
                    profileEmployeeWhichIsNotIncludeListSearchFilter.Filter_Employee_LastName = model.EmployeeWhichIsNotIncludeList.Filter.Filter_Employee_LastName;

                    model.EmployeeList.DataList = GetAllEmployeeByProfileId(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployeeSearchFilter);
                    model.EmployeeWhichIsNotIncludeList.DataList = GetAllEmployeeByProfileIdWhichIsNotIncluded(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployeeWhichIsNotIncludeListSearchFilter);

                }
                else
                {
                    model.EmployeeList = new DefinedEmployeeListViewModel();
                    model.EmployeeList.Filter = new DefinedEmployeeListFilterViewModel();
                    model.EmployeeList.DataList = new Business.Models.PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
                    model.EmployeeList.CurrentPage = 1;
                    model.EmployeeList.PageSize = 10;

                    model.EmployeeWhichIsNotIncludeList = new UndefinedEmployeeListViewModel();
                    model.EmployeeWhichIsNotIncludeList.Filter = new UndefinedEmployeeListFilterViewModel();
                    model.EmployeeWhichIsNotIncludeList.DataList = new Business.Models.PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
                    model.EmployeeWhichIsNotIncludeList.CurrentPage = 1;
                    model.EmployeeWhichIsNotIncludeList.PageSize = 10;
                }
                return View(model);
            }

            if (model.SubmitType == "Add")
            {
                if (model.EmployeeWhichIsNotIncludeList.DataList != null && model.ProfileId.HasValue)
                {
                    ModelState.Clear();
                    List<EmployeeCheckViewModel> records = model.EmployeeWhichIsNotIncludeList.DataList.Items.Where(x => x.Checked == true).ToList();
                    if (records != null)
                    {
                        foreach (var item in records)
                        {
                            ProfileEmployee profileEmployee = new ProfileEmployee();
                            profileEmployee.EmployeeId = item.Id;
                            profileEmployee.ProfileId = model.ProfileId.Value;
                            _profileEmployeeService.Add(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployee);
                        }
                    }
                }
            }
            if (model.SubmitType == "Delete")
            {
                if (model.EmployeeList.DataList != null && model.ProfileId.HasValue)
                {
                    ModelState.Clear();
                    List<EmployeeCheckViewModel> record = model.EmployeeList.DataList.Items.Where(x => x.Checked == true).ToList();
                    if (record != null)
                    {
                        foreach (var item in record)
                        {
                            var apiResponseModel = _profileEmployeeService.DeleteByProfileIdAndEmployeeId(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, model.ProfileId.Value, item.Id);
                        }
                    }
                }
            }

            model.ProfileSelectList = GetProfileSelectList(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar);
            if (model.ProfileId.HasValue)
            {
                if (model.EmployeeList.Filter == null)
                {
                    model.EmployeeList.Filter = new DefinedEmployeeListFilterViewModel();
                }
                if (model.EmployeeWhichIsNotIncludeList.Filter == null)
                {
                    model.EmployeeWhichIsNotIncludeList.Filter = new UndefinedEmployeeListFilterViewModel();
                }

                if (!model.EmployeeList.CurrentPage.HasValue)
                {
                    model.EmployeeList.CurrentPage = 1;
                }

                if (!model.EmployeeList.PageSize.HasValue)
                {
                    model.EmployeeList.PageSize = 10;
                }

                if (!model.EmployeeWhichIsNotIncludeList.CurrentPage.HasValue)
                {
                    model.EmployeeWhichIsNotIncludeList.CurrentPage = 1;
                }

                if (!model.EmployeeWhichIsNotIncludeList.PageSize.HasValue)
                {
                    model.EmployeeWhichIsNotIncludeList.PageSize = 10;
                }

                ProfileEmployeeSearchFilter profileEmployeeSearchFilter = new ProfileEmployeeSearchFilter();
                profileEmployeeSearchFilter.Filter_ProfileId = model.ProfileId.Value;
                profileEmployeeSearchFilter.CurrentPage = model.EmployeeList.CurrentPage.HasValue ? model.EmployeeList.CurrentPage.Value : 1;
                profileEmployeeSearchFilter.PageSize = model.EmployeeList.PageSize.HasValue ? model.EmployeeList.PageSize.Value : 10;
                profileEmployeeSearchFilter.SortOn = model.EmployeeList.SortOn;
                profileEmployeeSearchFilter.SortDirection = model.EmployeeList.SortDirection;
                profileEmployeeSearchFilter.Filter_Employee_Name = model.EmployeeList.Filter.Filter_Employee_Name;
                profileEmployeeSearchFilter.Filter_Employee_LastName = model.EmployeeList.Filter.Filter_Employee_LastName;

                ProfileEmployeeSearchFilter profileEmployeeWhichIsNotIncludeListSearchFilter = new ProfileEmployeeSearchFilter();
                profileEmployeeWhichIsNotIncludeListSearchFilter.Filter_ProfileId = model.ProfileId.Value;
                profileEmployeeWhichIsNotIncludeListSearchFilter.CurrentPage = model.EmployeeWhichIsNotIncludeList.CurrentPage.HasValue ? model.EmployeeWhichIsNotIncludeList.CurrentPage.Value : 1;
                profileEmployeeWhichIsNotIncludeListSearchFilter.PageSize = model.EmployeeWhichIsNotIncludeList.PageSize.HasValue ? model.EmployeeWhichIsNotIncludeList.PageSize.Value : 10;
                profileEmployeeWhichIsNotIncludeListSearchFilter.SortOn = model.EmployeeWhichIsNotIncludeList.SortOn;
                profileEmployeeWhichIsNotIncludeListSearchFilter.SortDirection = model.EmployeeWhichIsNotIncludeList.SortDirection;
                profileEmployeeWhichIsNotIncludeListSearchFilter.Filter_Employee_Name = model.EmployeeWhichIsNotIncludeList.Filter.Filter_Employee_Name;
                profileEmployeeWhichIsNotIncludeListSearchFilter.Filter_Employee_LastName = model.EmployeeWhichIsNotIncludeList.Filter.Filter_Employee_LastName;

                model.EmployeeList.DataList = GetAllEmployeeByProfileId(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployeeSearchFilter);
                model.EmployeeWhichIsNotIncludeList.DataList = GetAllEmployeeByProfileIdWhichIsNotIncluded(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployeeWhichIsNotIncludeListSearchFilter);
            }
            else
            {
                model.EmployeeList = new DefinedEmployeeListViewModel();
                model.EmployeeList.Filter = new DefinedEmployeeListFilterViewModel();
                model.EmployeeList.DataList = new Business.Models.PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
                model.EmployeeList.CurrentPage = 1;
                model.EmployeeList.PageSize = 10;

                model.EmployeeWhichIsNotIncludeList = new UndefinedEmployeeListViewModel();
                model.EmployeeWhichIsNotIncludeList.Filter = new UndefinedEmployeeListFilterViewModel();
                model.EmployeeWhichIsNotIncludeList.DataList = new Business.Models.PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
                model.EmployeeWhichIsNotIncludeList.CurrentPage = 1;
                model.EmployeeWhichIsNotIncludeList.PageSize = 10;
            }

            return View(model);
        }


        [NonAction]
        private PaginatedList<EmployeeCheckViewModel> GetAllEmployeeByProfileId(string userToken, string displayLanguage, ProfileEmployeeSearchFilter profileEmployeeSearchFilter)
        {
            //profile ait kullanıcı kayıtları listelenir
            // api'den çekim yapılacak
            var apiResponseModel = _profileEmployeeService.GetAllEmployeePaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployeeSearchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                if (apiResponseModel.Data != null && apiResponseModel.Data.Items != null)
                {
                    var employeeCheckList = apiResponseModel.Data.Items.Select(x => new EmployeeCheckViewModel()
                    {
                        Checked = false,
                        Id = x.Id,
                        Name = x.Name,
                        LastName = x.LastName,
                    }).ToList();
                    PaginatedList<EmployeeCheckViewModel> result = new PaginatedList<EmployeeCheckViewModel>(employeeCheckList, apiResponseModel.Data.TotalCount, apiResponseModel.Data.CurrentPage, apiResponseModel.Data.PageSize, apiResponseModel.Data.SortOn, apiResponseModel.Data.SortDirection);
                    return result;
                }
                else
                {
                    return new PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
                }
            }
            else
            {
                return new PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
            }
        }

        [NonAction]
        private PaginatedList<EmployeeCheckViewModel> GetAllEmployeeByProfileIdWhichIsNotIncluded(string userToken, string displayLanguage, ProfileEmployeeSearchFilter profileEmployeeWhichIsNotIncludeListSearchFilter)
        {
            //profile ait olmayan yetki kayıtları listelenir
            // api'den çekim yapılacak
            var apiResponseModel = _profileEmployeeService.GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(SessionHelper.CurrentUser.UserToken, SessionHelper.CurrentLanguageTwoChar, profileEmployeeWhichIsNotIncludeListSearchFilter);
            if (apiResponseModel.ResultStatusCode == ResultStatusCodeStatic.Success)
            {
                if (apiResponseModel.Data != null && apiResponseModel.Data.Items != null)
                {
                    var employeeCheckList = apiResponseModel.Data.Items.Select(x => new EmployeeCheckViewModel()
                    {
                        Checked = false,
                        Id = x.Id,
                        Name = x.Name,
                        LastName = x.LastName,
                    }).ToList();
                    PaginatedList<EmployeeCheckViewModel> result = new PaginatedList<EmployeeCheckViewModel>(employeeCheckList, apiResponseModel.Data.TotalCount, apiResponseModel.Data.CurrentPage, apiResponseModel.Data.PageSize, apiResponseModel.Data.SortOn, apiResponseModel.Data.SortDirection);
                    return result;
                }
                else
                {
                    return new PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
                }
            }
            else
            {
                return new PaginatedList<EmployeeCheckViewModel>(new List<EmployeeCheckViewModel>(), 0, 1, 10, "", "");
            }
        }


        [NonAction]
        private List<SelectListItem> GetProfileSelectList(string userToken, string displayLanguage)
        {
            // aktif profil kayıtları listelenir
            List<SelectListItem> resultList = new List<SelectListItem>();
            var apiResponseModel = _profileService.GetAll(userToken, displayLanguage);
            resultList = apiResponseModel.Data.OrderBy(r => displayLanguage == "tr" ? r.NameTR : r.NameEN).Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = displayLanguage == "tr" ? r.NameTR : r.NameEN }).ToList();
            return resultList;
        }
    }
}
