using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Models.Category
{
    public class CategorySearchFilter
    {
        // paging
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        // sorting
        public string SortOn { get; set; }
        public string SortDirection { get; set; }

        // filters        
        public string Filter_NameTR { get; set; }
        public string Filter_NameEN { get; set; }
    }
}
