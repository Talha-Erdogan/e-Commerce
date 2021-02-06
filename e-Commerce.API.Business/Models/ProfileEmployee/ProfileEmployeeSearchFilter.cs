using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.API.Business.Models.ProfileEmployee
{
    public class ProfileEmployeeSearchFilter
    {
        // paging
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        // sorting
        public string SortOn { get; set; }
        public string SortDirection { get; set; }

        // filters      
        public int Filter_ProfileId { get; set; }
        public string Filter_Employee_Name { get; set; }
        public string Filter_Employee_LastName { get; set; }
    }
}
