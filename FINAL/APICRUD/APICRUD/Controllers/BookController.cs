using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;
using APICRUD.EF;
using APICRUD.EF.Entities;

namespace APICRUD.Controllers
{
    public class BookController : ApiController
    {
        private readonly ContextFile db = new ContextFile();

        [HttpGet]
        [Route("api/Book/getall")]
        public HttpResponseMessage GetAll()
        {
            var data = db.Books.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        [Route("api/Book/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var data = db.Books.Find(id);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.OK, data);
            else return Request.CreateResponse(HttpStatusCode.NotFound, data);
        }
        [HttpPost]
        [Route("api/Book/create")]
        public HttpResponseMessage Create(Book c)
        {
            if (c == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }

            var authorExists = db.Authors.Any(a => a.AuthorId == c.AuthorId);
            if (!authorExists)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid AuthorId: Author does not exist");
            }
            db.Books.Add(c);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Book created successfully");
        }

        // PUT: api/Book/update/{id}
        [HttpPut]
        [Route("api/Book/update/{id}")]
        public HttpResponseMessage UpdateUser(int id, Book c)
        {
            if (c == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }
            var authorExists = db.Authors.Any(a => a.AuthorId == c.AuthorId);
            if (!authorExists)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid AuthorId: Author does not exist");
            }
            var book = db.Books.Find(id);
            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Book not found");
            }
            c.BookId = book.BookId;
            // Use SetValues for updating the entity
            db.Entry(book).CurrentValues.SetValues(c);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Book updated successfully");
        }

        // DELETE: api/Book/delete/{id}
        [HttpDelete]
        [Route("api/Book/delete/{id}")]
        public HttpResponseMessage DeleteUser(int id)
        {
            var book = db.Books.Find(id);
            if (book == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Book not found");
            }

            db.Books.Remove(book);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Book deleted successfully");
        }



    }
}
