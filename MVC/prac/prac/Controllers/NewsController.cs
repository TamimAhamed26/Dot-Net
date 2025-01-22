using prac.DTO;
using prac.EF;
using prac.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;


namespace APICRUD.Controllers
{
    public class NewsController : ApiController
    {
        private readonly NewsContext db = new NewsContext();


        News Convert(NewsDTO d)
        {
            return new News
            {
                NewsId = d.NewsId,
                Name = d.Name,
                Date = d.Date
            };
        }
        NewsDTO Convert(News d)
        {
            return new NewsDTO
            {
                NewsId = d.NewsId,
                Name = d.Name,
                Date = d.Date
            };
        }
        List<NewsDTO> Convert(List<News> News)
        {
            var list = new List<NewsDTO>();
            foreach (var item in News)
            {
                list.Add(Convert(item));
            }
            return list;
        }

        // GET: api/students/all
        [HttpGet]
        [Route("api/News/getall")]
        public HttpResponseMessage GetStudents()
        {
            var students = db.News.ToList();
            var studentDtos = Convert(students);
            return Request.CreateResponse(HttpStatusCode.OK, studentDtos);
        }

        // POST: api/students/create
        [HttpPost]
        [Route("api/News/create")]
        public HttpResponseMessage AddStudent(NewsDTO studentDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var student = Convert(studentDto);
            db.News.Add(student);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, "Student added successfully.");
        }

        // GET: api/students/{id}
        [HttpGet]
        [Route("api/News/{id}")]
        public HttpResponseMessage GetStudent(int id)
        {
            var student = db.News.Find(id);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            var studentDto = Convert(student);
            return Request.CreateResponse(HttpStatusCode.OK, studentDto);
        }

        // PUT: api/students/update/{id}
        [HttpPut]
        [Route("api/News/update/{id}")]
        public HttpResponseMessage UpdateStudent(int id, NewsDTO updatedStudentDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var student = db.News.Find(id);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            student.Name = updatedStudentDto.Name;
            student.Date = updatedStudentDto.Date;

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Student updated successfully.");
        }

        // DELETE: api/students/delete/{id}
        [HttpDelete]
        [Route("api/students/delete/{id}")]
        public HttpResponseMessage DeleteStudent(int id)
        {
            var student = db.News.Find(id);
            if (student == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Student not found.");
            }

            db.News.Remove(student);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Student deleted successfully.");
        }



    }
}
