using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SipayApi.Attributes;
using SipayApi.Extensions;
using SipayApi.Models;
using SipayApi.Services;
using System.Linq.Expressions;

namespace SipayApi.Controllers;


public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }

    public ApiResponse(T Data)
    {
        this.Data = Data;
        this.Success = true;
        this.Message = string.Empty;
    }
}

[ApiController]
[Route("sipy/api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;


    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("fake/{id}")] // Farklı bir yol belirledik
    public IActionResult GetFakeStudentById(int id)
    {
        // Fake student bilgilerini getirme işlemi burada gerçekleştirilecek
        // Bu işlem farklı bir yol ile çağrılabilir
        return Ok($"Fake student with Id {id}");
    }

    // GET: sipy/api/student
    [HttpGet]
    public IActionResult GetStudents()
    {
        var students = _studentService.GetAllStudents();
        return Ok(students);
    }

    // GET: sipy/api/student/{id}
    [HttpGet("{id}")]
    public IActionResult GetStudentById(int id)
    {
        var student = _studentService.GetStudentById(id);
        if (student == null)
        {
            return NotFound("Student not found");
        }
        var studentResponse = new StudentResponse
        {
            Id = student.Id,
            Name = student.Name,
            Lastname = student.Lastname,
            Age = student.Age,
            Email = student.Email
        };
        // StudentExtension methodu kullanımı
        string fullName = studentResponse.GetFullName(); 
        return Ok(new { FullName = fullName, Student = studentResponse });
    }

    // POST: sipy/api/student
    [HttpPost]
    public IActionResult CreateStudent([FromBody] StudentResponse studentResponse)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }

        var createdStudent = _studentService.CreateStudent(studentResponse);
        return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
    }

    // PUT: sipy/api/student/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateStudent(int id, [FromBody] StudentResponse studentResponse)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }

        var updatedStudent = _studentService.UpdateStudent(id, studentResponse);
        if (updatedStudent == null)
        {
            return NotFound("Student not found");
        }

        return Ok(updatedStudent);
    }

    // DELETE: sipy/api/student/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id)
    {
        if (!_studentService.DeleteStudent(id))
        {
            return NotFound("Student not found");
        }

        return NoContent();
    }

    // Hata durumunda dönecek 500 (Internal Server Error) hatasını eklemek için ExceptionHandler
    [HttpPost] // Veya [HttpGet] veya uygun diğer HTTP attribütleri
    [Route("error")]
    public IActionResult HandleError()
    {
        return StatusCode(500, "An error occurred");
    }

    // PATCH: sipy/api/student/{id}
    [HttpPatch("{id}")]
    public IActionResult PatchStudent(int id, [FromBody] JsonPatchDocument<StudentResponse> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest("Invalid data");
        }

        var studentToUpdate = _studentService.GetStudentById(id);
        if (studentToUpdate == null)
        {
            return NotFound("Student not found");
        }

        var studentResponse = new StudentResponse
        {
            Id = studentToUpdate.Id,
            Name = studentToUpdate.Name,
            Age = studentToUpdate.Age
        };

        patchDoc.ApplyTo(studentResponse, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }

        var updatedStudent = _studentService.UpdateStudent(id, studentResponse);
        if (updatedStudent == null)
        {
            return NotFound("Student not found");
        }

        return Ok(updatedStudent);
    }
    
}
