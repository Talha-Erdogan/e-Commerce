using e_Commerce.Web.Business.Models;
using e_Commerce.Web.Business.Models.Sex;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Interfaces
{
    public interface ISexService
    {
        ApiResponseModel<List<Sex>> GetAll(string userToken, string displayLanguage);

    }
}
