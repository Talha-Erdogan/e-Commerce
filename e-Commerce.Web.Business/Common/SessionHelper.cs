using e_Commerce.Web.Business.Common.Enums;
using e_Commerce.Web.Business.Common.Extensions;
using e_Commerce.Web.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace e_Commerce.Web.Business.Common
{
    public static class SessionHelper
    {

        // A Better Approach To Access HttpContext Outside A Controller In .Net Core 2.1
        // https://www.c-sharpcorner.com/article/a-better-approach-to-access-httpcontext-outside-a-controller-in-net-core-2-1/
        // Accessing HttpContext outside of framework components in ASP.NET Core
        // https://www.strathweb.com/2016/12/accessing-httpcontext-outside-of-framework-components-in-asp-net-core/


        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext CurrentHttpContext => _httpContextAccessor.HttpContext;


        public static SessionUser CurrentUser
        {
            get
            {

                if (CurrentHttpContext.Session.Get<SessionUser>("Broadcast_CurrentUser") == null)
                {
                    return null;
                }
                else
                {
                    return CurrentHttpContext.Session.Get<SessionUser>("Broadcast_CurrentUser");
                }
            }

            set
            {
                CurrentHttpContext.Session.Set<SessionUser>("Broadcast_CurrentUser", value);
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                //if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.TID))
                if (CurrentUser != null && CurrentUser.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                if (CurrentHttpContext.Session.GetString("CurrentLanguage") == null || string.IsNullOrEmpty(CurrentHttpContext.Session.GetString("CurrentLanguage").ToString()))
                {
                    string language = "tr-tr";
                    CurrentHttpContext.Session.SetString("CurrentLanguage", language);

                    return language;
                }
                else
                {
                    return CurrentHttpContext.Session.GetString("CurrentLanguage").ToString();
                }
            }

            set
            {
                CurrentHttpContext.Session.SetString("CurrentLanguage", value);
            }
        }

        public static string CurrentLanguageTwoChar
        {
            get
            {
                if (CurrentLanguage == "tr-tr")
                {
                    return "tr";
                }
                else if (CurrentLanguage == "en-us")
                {
                    return "en";
                }
                else
                {
                    return "tr";
                }
            }
        }

        public static void SetCurrentThreadCulture(string language)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        }

        // todo: Login metodu yeni projeye gore kodlanacak
        /*
        public static SessionLoginResult Login(string userRegNum, string userPasswordClear, bool isWindowsAuthentication, IUserService userService, IBshCasAuthService bshCasAuthService, ICustomAppUserService customAppUserService, IBshCasProfileService bshCasProfileService, IDepartmentService departmentService)
        */

        public static SessionLoginResult Login(string userName, string userPasswordClear,
            IAuthenticationService authenticationService, IProfileDetailService profileDetailService,
            IProfileEmployeeService profileEmployeeService)
        {


            var existUser = authenticationService.Login(userName, userPasswordClear, CurrentLanguageTwoChar);
            if (existUser.ResultStatusCode == ResultStatusCodeStatic.Error)
            {
                return new SessionLoginResult(false, existUser.ResultStatusMessage);
            }

            SessionUser currentUser = new SessionUser();
            currentUser.Id = existUser.Data.Id;
            currentUser.TRNationalId = existUser.Data.TRNationalId;
            currentUser.Name = existUser.Data.Name;
            currentUser.LastName = existUser.Data.LastName;

            currentUser.SexId = existUser.Data.SexId;

            currentUser.UserToken = existUser.Data.UserToken;


            var apiAuthResponse = profileDetailService.GetAllAuthByCurrentUser(existUser.Data.UserToken, CurrentLanguageTwoChar, existUser.Data.Id);
            if (apiAuthResponse.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return new SessionLoginResult(false, apiAuthResponse.ResultStatusMessage);
            }
            currentUser.AuthList = apiAuthResponse.Data;

            var apiProfileResponse = profileEmployeeService.GetAllProfileByCurrentUser(existUser.Data.UserToken, CurrentLanguageTwoChar, existUser.Data.Id);
            if (apiProfileResponse.ResultStatusCode != ResultStatusCodeStatic.Success)
            {
                return new SessionLoginResult(false, apiProfileResponse.ResultStatusMessage);
            }
            currentUser.ProfileList = apiProfileResponse.Data;









            // check username and password

            // SessionUser currentUser = new SessionUser();
            // currentUser.Id = 1; //existUser.Id;
            // currentUser.FirstName = "Name1"; // existUser.Name;
            // currentUser.LastName = "LastName1"; //existUser.Surname;
            // currentUser.NameSurname = "Name1 LastName1"; // existUser.NameSurname;
            // currentUser.Email = "name1@test.com"; //existUser.Email;
            // currentUser.RegNum = "1"; //existUser.RegNum;
            // currentUser.RegNumManager = "2"; //existUser.RegNumManager;
            // currentUser.RegNoManager = 2; // existUser.RegNoManager;
            // currentUser.PhoneMobile = "05071234567"; // existUser.PhoneMobile;
            // currentUser.TitleTxt = "Developer"; //existUser.TitleTxt;
            // currentUser.IsActive = true; //existUser.Status == "A" ? true : false;
            // currentUser.UserToken = "";
            // currentUser.UserName = "deneme";
            //currentUser.CompanyId = "ALUMINYUM";//DEFAULT olarak bu verildi. ilerleyen zamanda employye'inin company ıd 'si koyulacak.
            // //currentUser.CompanyId = "BILISIM";//DEFAULT olarak bu verildi. ilerleyen zamanda employye'inin company ıd 'si koyulacak.


            //currentUser.RoleId = 1;//default olarak 1 verildi.
            // uygulamada role tanımlaması yerine yetki bazlı çalışma bulunmaktadır

            // kullanıcıya ait yetki tanımları elde edilip session'a doldurulur
            // 111,222,333,444 sicil numarasına göre t_cas_user tablosundaki atanacak regno var mı kontrol edilecek

            //currentUser.UserBshCasAuthList = bshCasAuthService.GetAllByProjectCodeAndUserIdWhichAuthStatusIsActive(ConfigHelper.BshCasProjectCode, existUser.Id);
            //currentUser.UserBshCasProfileList = bshCasProfileService.GetAllByProjectCodeAndUserId(ConfigHelper.BshCasProjectCode, existUser.Id);

            //// oturum açan kullanıcının departman koduna göre appliance tracker'daki departman kaydı elde edilir
            //currentUser.ApplianceTrackerDeparment = departmentService.GetByBshCasDepartmentCodeWhichIsActiveTrue(existUser.DepartmentCode);
            //if (currentUser.ApplianceTrackerDeparment == null)
            //{
            //    return new SessionLoginResult(false, "Can not find departmant info from Appliance Tracker system for user.");
            //}

            /*
            // trace guid oluşturulması, session'a bu bilginin eklenmesi ve db tablosuna eklenmesi
            string traceGuid = Guid.NewGuid().ToString();
            UserSessionTrace userSessionTrace = new UserSessionTrace();
            userSessionTrace.CreatedDate = DateTime.Now;
            userSessionTrace.SessionTraceGuid = traceGuid;
            userSessionTrace.UserId = currentUser.Id;
            userSessionTrace.UserIpAddress = SessionHelper.GetCurrentRequestIpAddress();
            int resultUserSessionTrace = new UserSessionTraceService().Add(userSessionTrace);
            currentUser.UserSessionTrace_SessionTraceGuid = traceGuid;
            */

            CurrentUser = currentUser;

            return new SessionLoginResult(true, null);
        }


        public static SessionLoginResult LoginTest(string userName, string userPasswordClear)
        {
            // check username and password

            //CurrentUser = new User() { REF = 1, TID = "testuser1", NAME = "Test User1", ROLEREF = 1, ROLE = "Admin" };

            return new SessionLoginResult(true, null);
        }


        public static bool Logout()
        {
            var currentLanguage = SessionHelper.CurrentLanguage;
            CurrentHttpContext.Session.Clear();
            foreach (var item in CurrentHttpContext.Session.Keys)
            {
                CurrentHttpContext.Session.Remove(item);
            }
            SessionHelper.CurrentLanguage = currentLanguage;
            return true;
        }

        public static void SetCurrentLanguageAndCurrentThreadCultureByLanguageTwoChar(string languageTwoChar)
        {
            if (string.IsNullOrEmpty(languageTwoChar))
            {
                languageTwoChar = "tr";
            }
            string newLanguage = languageTwoChar.Equals("tr") ? "tr-tr" : "en-us";
            SessionHelper.CurrentLanguage = newLanguage;
            SessionHelper.SetCurrentThreadCulture(newLanguage);
        }

        public static string GetCurrentRequestIpAddress()
        {
            string requestIpAddress = "";

            /*
            if (!string.IsNullOrEmpty(CurrentHttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                requestIpAddress = CurrentHttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            else if (!string.IsNullOrEmpty(CurrentHttpContext.Request.ServerVariables["REMOTE_ADDR"]))
            {
                requestIpAddress = CurrentHttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                requestIpAddress = CurrentHttpContext.Request.UserHostAddress;
            }
            */

            if (CurrentHttpContext.Connection.RemoteIpAddress != null)
            {
                requestIpAddress = CurrentHttpContext.Connection.RemoteIpAddress.ToString();
            }

            return requestIpAddress;
        }


        public static bool CheckAuthForCurrentUser(params string[] authCodeList)
        {
            bool result = false;

            if (CurrentUser == null || CurrentUser.AuthList == null || CurrentUser.AuthList.Count == 0)
            {
                return result;
            }

            if (CurrentUser.AuthList.Where(r => authCodeList.Contains(r.Code)).Any())
            {
                result = true;
            }

            // todo: yeni projeye gore kodlanacak

            return result;
        }




        public static void SetObject(string key, object value)
        {
            var str = JsonConvert.SerializeObject(value);
            CurrentHttpContext.Session.SetString(key, str);
        }


        public static T GetObject<T>(string key) where T : class
        {
            string objectString = CurrentHttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(objectString))
            {
                return null;
            }
            else
            {
                T value = JsonConvert.DeserializeObject<T>(objectString);
                return value;
            }
        }


        public static async Task<string> GetUserToken()
        {
            return CurrentUser.UserToken;
        }

    }
}
