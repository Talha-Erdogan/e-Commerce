using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models
{
    public class ApiResponseModel<TData> : BaseResponseModel
    {
        public TData Data { get; set; }

    }
}
