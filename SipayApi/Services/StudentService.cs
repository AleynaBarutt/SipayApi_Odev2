using Microsoft.EntityFrameworkCore;
using SipayApi.Data;
using SipayApi.Models;
using System.Linq.Expressions;

namespace SipayApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;

        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentResponse CreateStudent(StudentResponse studentResponse)
        {
            var student = new Student { Name = studentResponse.Name, Lastname = studentResponse.Lastname, Age = studentResponse.Age,Email = studentResponse.Email };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();

            studentResponse.Id = studentResponse.Id;
            return studentResponse;
        }

        public StudentResponse UpdateStudent(int id, StudentResponse studentResponse)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return null;
            }

            student.Name = studentResponse.Name;
            student.Age = studentResponse.Age;
            _dbContext.SaveChanges();

            return studentResponse;
        }
        public StudentResponse GetStudentById(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return null;
            }

            return new StudentResponse { Name = student.Name, Lastname = student.Lastname, Age = student.Age };
        }
        public IEnumerable<StudentResponse> GetAllStudents()
        {
            return _dbContext.Students.Select(s => new StudentResponse
            {
                Id = s.Id.ToString(), // Id özelliğini seçiyoruz
                Name = s.Name,
                Lastname = s.Lastname,
                Age = s.Age,
                Email = s.Email
            });
        }

        public List<Student> GetByParameter(Expression<Func<Student, bool>> filterExpression)
        {
            return _dbContext.Students.Where(filterExpression).ToList();
        }
        public bool DeleteStudent(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return true;
        }

        

        

       
    }
}
