﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Models.Dtos
{
    public class StudentDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int studentId { get; set; }
        [Required, MaxLength(100)]
        public string firstName { get; set; }
        public string middleName { get; set; }
        [Required]
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public DateTime timeCreated { get; set; }
    }
}
