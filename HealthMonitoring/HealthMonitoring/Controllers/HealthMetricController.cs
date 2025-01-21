using BAL.DTOs;
using BAL.Services;
using HealthMonitoring.Auth;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System;
using System.Linq;
using System.Collections.Generic;

namespace API.Controllers
{
    [RoutePrefix("api/healthmetrics")]
    public class HealthMetricsController : ApiController
    {
        // POST: api/healthmetrics 
        [UserAccess]
        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreateHealthMetric(HealthMetricDTO healthMetricDto)
        {
            var username = AuthService.GetLoggedInUserName();

            if (string.IsNullOrEmpty(username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User not authenticated.");
            }

            if (healthMetricDto == null || healthMetricDto.Username != username)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data or user mismatch.");
            }

            bool isCreated = HealthMetricService.Create(healthMetricDto);
            if (isCreated)
            {
                return Request.CreateResponse(HttpStatusCode.Created, healthMetricDto);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to create health metric.");
            }
        }



        // GET: api/healthmetrics 
        [AdminAccess]
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetHealthMetrics()
        {
            var healthMetrics = HealthMetricService.Get();
            if (healthMetrics == null || healthMetrics.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            return Request.CreateResponse(HttpStatusCode.OK, healthMetrics);
        }

        // GET: api/healthmetrics/{id} 
        [AdminAccess]
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetHealthMetric(int id)
        {
            HealthMetricDTO healthMetric = HealthMetricService.Get(id);
            if (healthMetric == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Health metric not found.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, healthMetric);
        }

        // PUT: api/healthmetrics/{id} 
        [UserAccess]
        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateHealthMetric(HealthMetricDTO healthMetricDto)
        {
            var username = AuthService.GetLoggedInUserName();  // Get the logged-in username directly from the token

            if (healthMetricDto == null || healthMetricDto.Username != username)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data or user mismatch.");
            }

            bool isUpdated = HealthMetricService.Update(healthMetricDto);
            if (isUpdated)
            {
                return Request.CreateResponse(HttpStatusCode.OK, healthMetricDto);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to update health metric.");
            }
        }


        // DELETE: api/healthmetrics/{id} 
        [AdminAccess]
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage DeleteHealthMetric(int id)
        {
            bool isDeleted = HealthMetricService.Delete(id);
            if (isDeleted)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Health metric not found.");
            }
        }

        // GET: api/healthmetrics/metricsbydaterange (User only)
        [UserAccess]
        [HttpGet]
        [Route("metricsbydaterange")]
        public HttpResponseMessage GetMetricsByDateRange(DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();  // Get the logged-in user's username from the token

            var metrics = HealthMetricService.GetMetricsByDateRange(loggedInUsername, startDate, endDate);
            if (metrics == null || metrics.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No metrics found for the specified date range.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, metrics);
        }

        // GET: api/healthmetrics/average (User only)
        [UserAccess]
        [HttpGet]
        [Route("average")]
        public HttpResponseMessage GetAverageMetricValue(string metricType, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();  // Get the logged-in user's username from the token

            double average = HealthMetricService.GetAverageMetricValue(loggedInUsername, metricType, startDate, endDate);
            return Request.CreateResponse(HttpStatusCode.OK, average);
        }

        // GET: api/healthmetrics/minmax (User only)
        [UserAccess]
        [HttpGet]
        [Route("minmax")]
        public HttpResponseMessage GetMinMaxMetricValues(string metricType, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();  // Get the logged-in user's username from the token

            var minMax = HealthMetricService.GetMinMaxMetricValues(loggedInUsername, metricType, startDate, endDate);

          
            return Request.CreateResponse(HttpStatusCode.OK, new { MinValue = minMax.minValue, MaxValue = minMax.maxValue });
        }


        // GET: api/healthmetrics/trends (User only)
        [UserAccess]
        [HttpGet]
        [Route("trends")]
        public HttpResponseMessage GetMetricTrends(DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();  

            var trends = HealthMetricService.GetMetricTrends(loggedInUsername, startDate, endDate);
            if (trends == null || trends.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No trends found for the specified date range.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, trends);
        }

        [UserAccess]
        [HttpGet]
        [Route("history")]
        public HttpResponseMessage GetMetricHistory(string metricType, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName(); // Get logged-in user's username

            var history = HealthMetricService.GetMetricHistory(loggedInUsername, metricType, startDate, endDate);

            if (history == null || history.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, "No history found for the specified date range.");
            }

            // Create the response body with the dynamic message and data
            var result = new
            {
                message = $"The values for the graph are as below: X-axis is the date, Y-axis is the metric value, and the metric type is {metricType}.",
                data = history.Select(h => new
                {
                    date = h.Item1,
                    value = h.Item2
                })
            };

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


    }
}