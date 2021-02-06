using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace e_Commerce.API.Data.Entity
{
    [Table("ProfileDetails")]
    public class ProfileDetail
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        [Required]
        public int ProfileId { get; set; }
        [Required]
        public int AuthId { get; set; }
    }
}
