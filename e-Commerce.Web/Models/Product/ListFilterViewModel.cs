﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.Web.Models.Product
{
    public class ListFilterViewModel
    {
        // filters        
        public string NameTR { get; set; }
        public string NameEN { get; set; }
        public int? CategoryId { get; set; }
    }
}
