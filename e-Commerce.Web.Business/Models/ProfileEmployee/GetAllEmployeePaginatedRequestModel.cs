using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.ProfileEmployee
{
    public class GetAllEmployeePaginatedRequestModel : ListBaseRequestModel
    {
        // filters      
        public int ProfileId { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_LastName { get; set; }
    }
}
