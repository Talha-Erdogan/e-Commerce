using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.Web.Models.Product
{
    public class ListForCustomerViewModel
    {
        public Business.Models.Product.ProductSearchForCustomer Filter { get; set; }

        public List<Business.Models.Product.ProductWithDetail> DataList { get; set; }


        public string CurrentLanguageTwoChar { get; set; }
        //select list items
        public List<SelectListItem> FilterCategorySelectList { get; set; }

    }
}
