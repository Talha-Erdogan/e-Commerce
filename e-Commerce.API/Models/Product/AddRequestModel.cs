using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.API.Models.Product
{
    public class AddRequestModel
    {
        public long Id { get; set; }

        public string NameTR { get; set; }

        public string NameEN { get; set; }

        public string DescriptionTR { get; set; }

        public string DescriptionEN { get; set; }

        public string ImageFilePath { get; set; }

        public int CategoryId { get; set; }
    }
}
