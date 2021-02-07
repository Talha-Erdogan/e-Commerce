using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.ProfileEmployee
{
    public class AddRequestModel
    {
        public long Id { get; set; }
        public int ProfileId { get; set; }
        public int EmployeeId { get; set; }
    }
}
