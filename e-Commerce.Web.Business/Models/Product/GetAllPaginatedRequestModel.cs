using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Product
{
    public class GetAllPaginatedRequestModel : ListBaseRequestModel
    {
        // filters        
        public string NameTR { get; set; }
        public string NameEN { get; set; }
        public int? CategoryId { get; set; }
    }
}
