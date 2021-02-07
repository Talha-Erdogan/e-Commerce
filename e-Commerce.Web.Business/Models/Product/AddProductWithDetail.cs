using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Product
{
    public class AddProductWithDetail
    {
        public long Id { get; set; }

        public string NameTR { get; set; }

        public string NameEN { get; set; }

        public string DescriptionTR { get; set; }

        public string DescriptionEN { get; set; }

        public string ImageFilePath { get; set; }

        public ImageInformation ImageInformation { get; set; } // 2 asamada kayıt için parametre olarak business'a gönderiyoruz. Sonraki aşamalar da burası ImageInformation yapılabilir. İlk istekte, resimler kaydedilip, başarılı kayıt olduktan sonra diğer bilgiler kaydedilir.


        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public int? DeletedBy { get; set; }
    }
}
