using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace e_Commerce.API.Data.Entity
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(100)]
        public string NameTR { get; set; }

        [StringLength(100)]
        public string NameEN { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedBy { get; set; }

    }
}
