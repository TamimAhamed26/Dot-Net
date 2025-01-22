using intoWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace intoWebApi.Controllers
{
    //custom route
    public class PersonController : ApiController
    {

        /*
        [HttpGet]
        [Route("api/person/all")] //not / is allowed at begining
        public HttpResponseMessage GetAllPerson()
        {
            string[] names = { "Tanvir", "Sabbir", "Rahim", "Karim" };
            return Request.CreateResponse(HttpStatusCode.OK, names);
        }
        [HttpGet]
        [Route("api/person/{p_id}/name/{p_name}")]
        public HttpResponseMessage GetPerson(int p_id, string p_name)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPost]
        [Route("api/person/create")]
        public HttpResponseMessage Add(Person p)
        {


            return Request.CreateResponse(HttpStatusCode.OK);
        }

 */
//the previous apprach didnt have db or temp storage so couldnt see the values.


    private static readonly List<Person> persons = new List<Person>();//temp storage will reset after restarting the application

        [HttpGet]
        [Route("api/person/all")]
        public HttpResponseMessage GetAllPerson()
        {
            // Return the list of all persons
            return Request.CreateResponse(HttpStatusCode.OK, persons);
        }

        [HttpGet]
        [Route("api/person/{p_id}/name/{p_name}")]
        public HttpResponseMessage GetPerson(int p_id, string p_name)
        {
            // Find a person by ID and name
            var person = persons.Find(p => p.Id == p_id && p.Name == p_name);
            if (person != null)
                return Request.CreateResponse(HttpStatusCode.OK, person);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, "Person not found");
        }

        [HttpPost]
        [Route("api/person/create")]
        public HttpResponseMessage Add(Person p)
        {
            // Add the new person to the list
            persons.Add(p);
            return Request.CreateResponse(HttpStatusCode.OK, "Person added successfully");
        }


    }
}