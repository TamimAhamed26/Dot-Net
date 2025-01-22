using BLL.Services; // Assuming your business logic layer is in BLL.Services
using BAL.DTOs; // Assuming your DTOs are in the BAL.DTOs namespace
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;

namespace WebAPI.Controllers
{
    public class EventController : ApiController
    {
    
        [HttpGet]
        [Route("api/event/all")]
        public HttpResponseMessage Get()
        {
            var data = EventService.Get();  
            return Request.CreateResponse(HttpStatusCode.OK, data); 
        }


        [HttpGet]
        [Route("api/event/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var data = EventService.Get(id); 
            if (data != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, data);  
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Event not found.");  
        }

        // Search events by name
        [HttpGet]
        [Route("api/event/search/{name}")]
        public HttpResponseMessage Search(string name)
        {
            var data = EventService.SearchEventsByName(name);  
            return Request.CreateResponse(HttpStatusCode.OK, data);  
        }

        
        [HttpGet]
        [Route("api/event/upcoming/{date}")]
        public HttpResponseMessage GetUpcomingEvents(DateTime date)
        {
            var data = EventService.GetUpcomingEvents(date); 
            return Request.CreateResponse(HttpStatusCode.OK, data);  
        }
    }
}
