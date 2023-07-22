using SipayApi.Models;
using System.Linq.Expressions;

namespace SipayApi.Services
{
     
    public interface IStudentService
    {
        List<Student> GetByParameter(Expression<Func<Student, bool>> filterExpression);
        IEnumerable<StudentResponse> GetAllStudents();
        StudentResponse GetStudentById(int id);
        StudentResponse CreateStudent(StudentResponse studentResponse);
        StudentResponse UpdateStudent(int id, StudentResponse studentResponse);
        bool DeleteStudent(int id);
    }

    
}
