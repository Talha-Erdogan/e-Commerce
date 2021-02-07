using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.API.Models.Category
{
    public class GetAllPaginatedRequestModel : ListBaseRequestModel
    {
        public string NameTR { get; set; }
        public string NameEN { get; set; }
    }
}
