using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace e_Commerce.API.Data.Entity
{
    [Table("Sex")]
    public class Sex
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(50)]
        public string NameTR { get; set; }

        [StringLength(50)]
        public string NameEN { get; set; }

    }
}
