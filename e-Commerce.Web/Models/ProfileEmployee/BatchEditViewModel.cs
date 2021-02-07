using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.Web.Models.ProfileEmployee
{
    public class BatchEditViewModel
    {
        public int? ProfileId { get; set; }
        public List<SelectListItem> ProfileSelectList { get; set; }
        public DefinedEmployeeListViewModel EmployeeList { get; set; }
        public UndefinedEmployeeListViewModel EmployeeWhichIsNotIncludeList { get; set; }

        public string SubmitType { get; set; }
    }
}
