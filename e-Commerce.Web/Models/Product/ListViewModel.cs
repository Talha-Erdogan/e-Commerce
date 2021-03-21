using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.Web.Models.Product
{
    public class ListViewModel : ListBaseViewModel<Business.Models.Product.ProductWithDetail, ListFilterViewModel>
    {
        public List<SelectListItem> FilterCategorySelectList { get; set; }
    }
}
