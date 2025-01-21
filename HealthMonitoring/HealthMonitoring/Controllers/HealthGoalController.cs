using BAL.DTOs;
using BAL.Services;
using HealthMonitoring.Auth;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [RoutePrefix("api/healthgoals")]
    public class HealthGoalsController : ApiController
    {
        // POST: api/healthgoals
        [UserAccess]
        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreateHealthGoal(HealthGoalDTO healthGoalDto)
        {
            var username = AuthService.GetLoggedInUserName();

            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User not authenticated.");
            }

            if (healthGoalDto == null || healthGoalDto.Username != username)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data or user mismatch.");
            }

            bool isCreated = HealthGoalService.Create(healthGoalDto);
            if (isCreated)
            {
                return Request.CreateResponse(HttpStatusCode.Created, healthGoalDto);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to create health goal.");
            }
        }

        // GET: api/healthgoals
        [UserAccess]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetHealthGoals()
        {
            var username = AuthService.GetLoggedInUserName(); 

            var healthGoals = HealthGoalService.GetAllByUser(username);
            if (healthGoals == null || healthGoals.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No health goals found for the user.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, healthGoals);
        }

        // GET: api/healthgoals/{id}
        [UserAccess]
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetHealthGoal(int id)
        {
            var username = AuthService.GetLoggedInUserName(); 

            var healthGoal = HealthGoalService.GetByUserAndId(username, id);
            if (healthGoal == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Health goal not found for the user.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, healthGoal);
        }

        // PUT: api/healthgoals
        [UserAccess]
        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateHealthGoal(HealthGoalDTO healthGoalDto)
        {
            var username = AuthService.GetLoggedInUserName();

            if (healthGoalDto == null || healthGoalDto.Username != username)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data or user mismatch.");
            }

            bool isUpdated = HealthGoalService.Update(healthGoalDto);
            if (isUpdated)
            {
                return Request.CreateResponse(HttpStatusCode.OK, healthGoalDto);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to update health goal.");
            }
        }

        // DELETE: api/healthgoals/{id}
        [UserAccess]
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage DeleteHealthGoal(int id)
        {
            var username = AuthService.GetLoggedInUserName();

            bool isDeleted = HealthGoalService.DeleteByUserAndId(username, id);
            if (isDeleted)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Health goal not found for the user.");
            }
        }

        // PUT: api/healthgoals/updateprogress
        [UserAccess]
        [HttpPut]
        [Route("updateprogress")]
        public HttpResponseMessage UpdateProgress()
        {
            var username = AuthService.GetLoggedInUserName();

            HealthGoalService.UpdateProgress(username);

            return Request.CreateResponse(HttpStatusCode.OK, "Progress updated successfully.");
        }
    }
}
