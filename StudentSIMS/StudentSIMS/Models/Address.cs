using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int addressId { get; set; }
        [Required]
        public int studentId { get; set; }
        [Required]
        public int streetNumber { get; set; }
        [Required]
        public string street { get; set; }
        [Required]
        public string suburb { get; set; }
        [Required]
        public string city { get; set; }
        public int postCode { get; set; }
        [Required]
        public string country { get; set; }
        public DateTime timeCreated { get; set; }

        [ForeignKey("studentId")]
        public Student Student { get; set; }
    }
}
