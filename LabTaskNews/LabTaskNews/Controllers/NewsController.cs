using BAL.DTOs;
using BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LabTaskNews.Controllers
{
    public class NewsController : ApiController
    {

        [HttpGet]
        [Route("api/news/all")]
        public HttpResponseMessage Get()
        {
            var data = NewsService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpGet]
        [Route("api/news/{id}")]
        public HttpResponseMessage GetNews(int id)
        {
            try
            {
                var data = NewsService.GetNewsById(id);
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "news not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/news/create")]
        public HttpResponseMessage Create(NewsDTO data)
        {
            try
            {
                var result = NewsService.CreateNews(data);
                if (result == "news added successfully!")
                {
                    return Request.CreateResponse(HttpStatusCode.Created, result);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Update an existing customer
        [HttpPut]
        [Route("api/news/update/{id}")]
        public HttpResponseMessage Update(int id, NewsDTO data)
        {
            try
            {
                data.Id = id;
                var result = NewsService.UpdateNews(data);
                if (result == "news updated successfully!")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "news not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Delete a customer
        [HttpDelete]
        [Route("api/news/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var result = NewsService.DeleteNews(id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "news deleted successfully.");
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "news not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet]
        [Route("api/news/search")]
        public HttpResponseMessage Search(string title = null, string category = null, DateTime? date = null)
        {
            try
            {
                var data = NewsService.SearchNews(title, category, date);
                if (data == null || data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No matching news found.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }



    }
}

