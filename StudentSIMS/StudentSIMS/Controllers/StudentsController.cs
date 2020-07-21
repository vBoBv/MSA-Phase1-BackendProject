using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSIMS.Models;
using StudentSIMS.Models.Dtos;
using StudentSIMS.Repository.IRepository;

namespace StudentSIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepo, IMapper mapper)
        {
            _studentRepo = studentRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of students
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentDto>))]
        public IActionResult GetStudents()
        {
            var objList = _studentRepo.GetStudents();
            var objDto = new List<StudentDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<StudentDto>(obj));
            }

            return Ok(objList);
        }

        /// <summary>
        /// Get individual student
        /// </summary>
        /// <param name="studentId">The id of the student</param>
        /// <returns></returns>
        [HttpGet("{studentId:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StudentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetStudent(int studentId)
        {
            var obj = _studentRepo.GetStudent(studentId);
            if (obj == null)
            {
                return NotFound();
            }

            //var objDto = new StudentDto()
            //{
            //    studentId = obj.studentId,
            //    firstName = obj.firstName,
            //    lastName = obj.lastName
            //};

            var objDto = _mapper.Map<StudentDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Add a new student
        /// </summary>
        /// <param name="studentDto">The properties of a student</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<StudentDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateStudent([FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
            {
                return BadRequest(ModelState);
            }

            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var studentObj = _mapper.Map<Student>(studentDto);
            if (!_studentRepo.CreateStudent(studentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {studentObj.firstName}");
                return StatusCode(500, ModelState);
            }
            //return Ok();
            return CreatedAtAction("GetStudent", new { studentId = studentObj.studentId }, studentObj);
        }

        /// <summary>
        /// Update a student
        /// </summary>
        /// <param name="studentId">The id of the student to be updated</param>
        /// <param name="studentDto">The property of the student to be updated</param>
        /// <returns></returns>
        [HttpPatch("{studentId:int}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStudent(int studentId, [FromBody] StudentDto studentDto)
        {
            if (studentDto == null || studentId != studentDto.studentId)
            {
                return BadRequest(ModelState);
            }

            var studentObj = _mapper.Map<Student>(studentDto);
            
            if (!_studentRepo.UpdateStudent(studentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {studentObj.firstName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="studentId">The id of the student to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{studentId:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStudent(int studentId)
        {
            if (!_studentRepo.StudentExists(studentId))
            {
                return NotFound();
            }

            var studentObj = _studentRepo.GetStudent(studentId);
            if (!_studentRepo.DeleteStudent(studentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {studentObj.firstName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
