using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace e_Commerce.API.Data.Entity
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NameTR { get; set; }

        [Required]
        [StringLength(100)]
        public string NameEN { get; set; }

        [StringLength(500)]
        public string DescriptionTR { get; set; }

        [StringLength(500)]
        public string DescriptionEN { get; set; }

        [StringLength(500)]
        public string ImageFilePath { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedDateTime { get; set; }

        public int? DeletedBy { get; set; }
    }
}
