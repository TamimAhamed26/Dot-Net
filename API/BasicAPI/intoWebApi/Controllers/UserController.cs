using intoWebApi.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace intoWebApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly StudentEntitie db = new StudentEntitie();

        // GET: api/user/all
        [HttpGet]
        [Route("api/user/all")]
        public HttpResponseMessage AllUsers()
        {
            var data = db.Users.ToList(); // Fetch all users
            return Request.CreateResponse(HttpStatusCode.OK, data); // Send the data in the response
        }

        // GET: api/student/{id}
        [HttpGet]
        [Route("api/user/{id}")]
        public HttpResponseMessage SingleUser(int id)
        {
            var user = db.Users.Find(id); // Find user by ID
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, user); // Return user details
        }

        // POST: api/student/create
        [HttpPost]
        [Route("api/user/create")]
        public HttpResponseMessage CreateStudent(User user)
        {
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }

            db.Users.Add(user); // Add new user
            db.SaveChanges();   // Save changes to the database
            return Request.CreateResponse(HttpStatusCode.Created, "User created successfully");
        }
        // PUT: api/user/update/{id}
        [HttpPut]
        [Route("api/user/update/{id}")]
        public HttpResponseMessage UpdateUser(int id, User updatedUser)
        {
            if (updatedUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
            }
            updatedUser.UserId = user.UserId;
            // Use SetValues for updating the entity
            db.Entry(user).CurrentValues.SetValues(updatedUser);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "User updated successfully");
        }

        // DELETE: api/user/delete/{id}
        [HttpDelete]
        [Route("api/user/delete/{id}")]
        public HttpResponseMessage DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "User deleted successfully");
        }


    }
}