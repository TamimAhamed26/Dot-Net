using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL.DTOs;
using BAL.Services;
using HealthMonitoring.Auth;

namespace HealthMonitoring.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User data is null.");
            }
            var result = UserService.RegisterUser(userDTO);

            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.Created, "Wait for admin approval.");
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User registration failed.");
        }
        [HttpPost]
        [AdminAccess]
        [Route("verify/{username}")]
        public HttpResponseMessage VerifyUser(string username)
        {
            var result = UserService.VerifyUser(username);

            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "User verified successfully.");
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");
        }

        [HttpDelete]
        [AdminAccess]
        [Route("deny/{username}")]
        public HttpResponseMessage DenyUser(string username)
        {
            var result = UserService.DenyUser(username);

            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "User denied and removed.");
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not found.");
        }
    }
}
