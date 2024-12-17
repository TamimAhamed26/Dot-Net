using CFapiMigration.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CFapiMigration.DTO;
using System.Web.UI;

namespace CFapiMigration.Controllers
{
    public class StudentController : ApiController
    {
        private readonly ContextFile db = new ContextFile();

        Student Convert(StudentDTO d)
        {
            return new Student
            {
                Id = d.Id,
                Name = d.Name,
                Age=d.Age
            };
        }
        StudentDTO Convert(Student d)
        {
            return new StudentDTO
            {
                Id = d.Id,
                Name = d.Name,
                Age = d.Age
            };
        }

        List<StudentDTO> Convert(List<Student> students)
        {
            var list = new List<StudentDTO>();
            foreach (var item in students)
            {
                list.Add(Convert(item));
            }
            return list;
        }

        // GET: api/students/all
        [HttpGet]
        [Route("api/students/all")]
        public HttpResponseMessage GetStudents()
        {
            var students = db.Students.ToList();
            var studentDtos = Convert(students);
            return Request.CreateResponse(HttpStatusCode.OK, studentDtos);
        }

        // POST: api/students/create
        [HttpPost]
        [Route("api/students/create")]
        public HttpResponseMessage AddStudent(StudentDTO studentDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var student = Convert(studentDto);
            db.Students.Add(student);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, "Student added successfully.");
        }

        // GET: api/students/{id}
        [HttpGet]
        [Route("api/students/{id}")]
        public HttpResponseMessage GetStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            var studentDto = Convert(student);
            return Request.CreateResponse(HttpStatusCode.OK, studentDto);
        }

        // PUT: api/students/update/{id}
        [HttpPut]
        [Route("api/students/update/{id}")]
        public HttpResponseMessage UpdateStudent(int id, StudentDTO updatedStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var student = db.Students.Find(id);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            student.Name = updatedStudentDto.Name;
            student.Age = updatedStudentDto.Age;

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Student updated successfully.");
        }

        // DELETE: api/students/delete/{id}
        [HttpDelete]
        [Route("api/students/delete/{id}")]
        public HttpResponseMessage DeleteStudent(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            db.Students.Remove(student);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Student deleted successfully.");
        }

    }
}
