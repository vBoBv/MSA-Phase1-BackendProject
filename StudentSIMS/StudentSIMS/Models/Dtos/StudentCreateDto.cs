using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Models.Dtos
{
    public class StudentCreateDto
    {
        [Required, MaxLength(100)]
        public string firstName { get; set; }
        public string middleName { get; set; }
        [Required]
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
    }
}
