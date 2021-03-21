using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Models.Product
{
    public class ProductWithDetail : Data.Entity.Product
    {
        //category column
        public string Category_NameTR { get; set; }

        public string Category_NameEN { get; set; }
    }
}
