using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Models.Product
{
    public class ProductSearchForCustomer
    {  
        // filters        
        public string Filter_NameTR { get; set; }
        public string Filter_NameEN { get; set; }
        public int? Filter_CategoryId { get; set; }
    }
}
