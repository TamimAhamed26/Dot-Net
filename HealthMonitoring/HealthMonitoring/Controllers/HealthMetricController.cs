using BAL.DTOs;
using BAL.Services;
using HealthMonitoring.Auth;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    [UserAccess]
    [RoutePrefix("api/healthmetrics")]
    public class HealthMetricsController : ApiController
    {
        // POST: api/healthmetrics
        [HttpPost]
        [Route("")]
        public HttpResponseMessage CreateHealthMetric(HealthMetricDTO healthMetricDto)
        {
            if (healthMetricDto == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
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
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetHealthMetrics()
        {
            List<HealthMetricDTO> healthMetrics = HealthMetricService.Get();
            if (healthMetrics == null || healthMetrics.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            return Request.CreateResponse(HttpStatusCode.OK, healthMetrics);
        }

        // GET: api/healthmetrics/{id}
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
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage UpdateHealthMetric(int id, HealthMetricDTO healthMetricDto)
        {
            if (healthMetricDto == null || id != healthMetricDto.Id)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
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
    }
}
