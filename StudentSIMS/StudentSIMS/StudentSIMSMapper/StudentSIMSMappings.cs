using AutoMapper;
using StudentSIMS.Models;
using StudentSIMS.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.StudentSIMSMapper
{
    public class StudentSIMSMappings: Profile
    {
        public StudentSIMSMappings()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, StudentCreateDto>().ReverseMap();
            CreateMap<Student, StudentUpdateDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Address, AddressCreateDto>().ReverseMap();
            CreateMap<Address, AddressUpdateDto>().ReverseMap();
        }
    }
}
