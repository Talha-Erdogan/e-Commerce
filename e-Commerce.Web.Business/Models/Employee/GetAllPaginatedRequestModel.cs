using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Employee
{
    public class GetAllPaginatedRequestModel : ListBaseRequestModel
    {
        // filters        
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
