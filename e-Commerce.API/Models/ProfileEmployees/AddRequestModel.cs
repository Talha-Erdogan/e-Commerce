using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.API.Models.ProfileEmployees
{
    public class AddRequestModel
    {
        public long Id { get; set; }
        public int ProfileId { get; set; }
        public int EmployeeId { get; set; }
    }
}
