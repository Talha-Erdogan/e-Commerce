using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace e_Commerce.API.Data.Entity
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(50)]
        public string TRNationalId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [Required]
        public int SexId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }


    }
}
