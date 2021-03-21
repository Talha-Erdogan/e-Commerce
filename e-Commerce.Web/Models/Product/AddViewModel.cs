using e_Commerce.Web.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.Web.Models.Product
{
    public class AddViewModel
    {
        public long Id { get; set; }

        public string NameTR { get; set; }

        public string NameEN { get; set; }

        public string DescriptionTR { get; set; }

        public string DescriptionEN { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile ImageFilePath { get; set; }

        public List<SelectListItem> CategorySelectList { get; set; }

        // others
        public string DisplayLanguage { get; set; }
        public string SubmitType { get; set; }

        //Herhangi bir hata oldugunda, sessiondaki resmin eklenmesi işlevidir.
        public string SessionImageFileName { get; set; }//sessionda tutulan resmin ismi
        public ImageInformation ImageInformation { get; set; } //resim bilgileri
        public string SessionGuid { get; set; } //resmi eklerken, session guid ile ekliyoruz ki, bellekte kalan image verisi tekrar tekrar eklenmesin

    }
}
