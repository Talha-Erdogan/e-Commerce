using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Category
{
    public class GetAllPaginatedRequestModel : ListBaseRequestModel
    {
        public string NameTR { get; set; }
        public string NameEN { get; set; }
    }
}
