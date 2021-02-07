using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_Commerce.Web.Models.Auth
{
    public class AddViewModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(150)]
        public string NameTR { get; set; }

        [StringLength(150)]
        public string NameEN { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public int DeletedBy { get; set; }

        //edit or delete 
        public string SubmitType { get; set; }
    }
}
