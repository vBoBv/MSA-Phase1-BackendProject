using StudentSIMS.Data;
using StudentSIMS.Models;
using StudentSIMS.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSIMS.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _db;

        public StudentRepository(StudentContext db)
        {
            _db = db;
        }

        public bool CreateStudent(Student student)
        {
            _db.Student.Add(student);
            return Save();
        }

        public bool DeleteStudent(Student student)
        {
            _db.Student.Remove(student);
            return Save();
        }

        public Student GetStudent(int studentId)
        {
            return _db.Student.FirstOrDefault(s => s.studentId == studentId); ;
        }

        public ICollection<Student> GetStudents()
        {
            return _db.Student.OrderBy(s => s.firstName).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool StudentExists(int id)
        {
            return _db.Student.Any(s => s.studentId == id);
        }

        public bool UpdateStudent(Student student)
        {
            _db.Student.Update(student);
            return Save();
        }
    }
}