﻿using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Product
{
    public class ProductSearchFilter
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
        public int? Filter_CategoryId { get; set; }
    }
}
