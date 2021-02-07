using System;
using System.Collections.Generic;
using System.Text;

namespace e_Commerce.Web.Business.Models.Profile
{
    public class Profile
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameTR { get; set; }
        public string NameEN { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedBy { get; set; }
    }
}
