using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.API.Models
{
    public class BaseResponseModel
    {
        public string ResultStatusCode { get; set; }
        public string ResultStatusMessage { get; set; }
        public string DisplayLanguage { get; set; }
    }
}
