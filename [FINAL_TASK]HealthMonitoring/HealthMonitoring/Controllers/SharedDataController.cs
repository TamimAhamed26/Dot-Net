using BAL.DTOs;
using BAL.Services;
using HealthMonitoring.Auth;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/shareddata")]
    public class SharedDataController : ApiController
    {
        // POST: api/shareddata
        [UserAccess]
        [HttpPost]
        [Route("")]
        public HttpResponseMessage ShareData(SharedDataDTO sharedDataDto)
        {
            var username = AuthService.GetLoggedInUserName();

            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User not authenticated.");
            }

            if (sharedDataDto == null || string.IsNullOrEmpty(sharedDataDto.SharedWith) || sharedDataDto.ExpiryDate <= DateTime.UtcNow)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid shared data request.");
            }

            bool isShared = SharedDataService.ShareData(username, sharedDataDto.SharedWith, sharedDataDto.ExpiryDate);

            if (isShared)
            {
                return Request.CreateResponse(HttpStatusCode.Created, "Health data successfully shared.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to share health data.");
            }
        }

        // GET: api/shareddata?sharedWith={email}
        [AdminAccess]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetSharedData(string sharedWith)
        {
            if (string.IsNullOrEmpty(sharedWith))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Healthcare provider email is required.");
            }

            var sharedData = SharedDataService.GetSharedData(sharedWith);

            if (sharedData == null || sharedData.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No shared data found for the specified healthcare provider.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, sharedData);
        }

        // DELETE: api/shareddata/{id}
        [UserAccess]
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage RevokeAccess(int id)
        {
            if (id <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid shared data ID.");
            }

            bool isRevoked = SharedDataService.RevokeAccess(id);

            if (isRevoked)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Access revoked successfully.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Shared data not found or already revoked.");
            }
        }

        // GET: api/shareddata/validate?token={token}
        [HttpGet]
        [Route("validate")]
        public HttpResponseMessage ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token is required.");
            }

            bool isValid = SharedDataService.ValidateToken(token);

            if (isValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Token is valid.");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token is invalid or expired.");
            }
        }
    }
}
