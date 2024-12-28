using APICRUD.EF.Entities;
using APICRUD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace APICRUD.Controllers
{
    public class AuthorController : ApiController
    {

        private readonly ContextFile db = new ContextFile();

        [HttpGet]
        [Route("api/Author/getall")]
        public HttpResponseMessage GetAll()
        {
            var data = db.Authors.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        [Route("api/Author/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var data = db.Authors.Find(id);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.OK, data);
            else return Request.CreateResponse(HttpStatusCode.NotFound, data);
        }
        [HttpPost]
        [Route("api/Author/create")]
        public HttpResponseMessage Create(Author c)
        {
            if (c == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }

            
            db.Authors.Add(c);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Author created successfully");
        }

        // PUT: api/Book/update/{id}
        [HttpPut]
        [Route("api/Author/update/{id}")]
        public HttpResponseMessage UpdateUser(int id, Author c)
        {
            if (c == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }
          
            var Author = db.Authors.Find(id);
            if (Author == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Author not found");
            }
            c.AuthorId = Author.AuthorId;
            // Use SetValues for updating the entity
            db.Entry(Author).CurrentValues.SetValues(c);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Author updated successfully");
        }

        // DELETE: api/Book/delete/{id}
        [HttpDelete]
        [Route("api/Author/delete/{id}")]
        public HttpResponseMessage DeleteUser(int id)
        {
            var Author = db.Authors.Find(id);
            if (Author == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Author not found");
            }
            //cascade delete from EF
            db.Authors.Remove(Author);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Author deleted successfully");
        }




    }
}
