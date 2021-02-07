using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Common
{
    public static class ConfigHelper
    {
        private static IConfiguration _configuration { get; set; }

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string ApiUrl
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["PortalApiUrl"];
                // todo: appsettings.json dosyasından okunması sağlanacak
                var result= _configuration.GetSection("AppSettings:ApiUrl");
                return result.Value;
            }
        }

        public static string ApiBaseUrl
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["PortalApiUrl"];
                // todo: appsettings.json dosyasından okunması sağlanacak
                var result = _configuration.GetSection("AppSettings:ApiBaseUrl");
                return result.Value;
            }
        }

        public static string LogSaveType
        {
            get
            {
                //return System.Configuration.ConfigurationManager.AppSettings["LogSaveType"]; 
                // todo: appsettings.json dosyasından okunması sağlanacak
                return "LogSaveType";
            }
        }

    }
}
