using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models
{
    public class BaseResponseModel
    {
        public string ResultStatusCode { get; set; }
        public string ResultStatusMessage { get; set; }

        public string DisplayLanguage { get; set; }

        public List<object> ErrorMessageList { get; set; }

    }
}
