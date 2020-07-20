using StudentSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Repository.IRepository
{
    public interface IStudentRepository
    {
        ICollection<Student> GetStudents();
        Student GetStudent(int studentId);
        bool CreateStudent(Student student);
        bool UpdateStudent(Student student);
        bool DeleteStudent(Student student);
        bool StudentExists(int id);
        bool Save();
    }
}
