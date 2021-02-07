﻿using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Product
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
