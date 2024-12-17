using BAL.DTOs;
using BAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppLayerAPI.Controllers
{
    public class ProductController : ApiController
    {
        // Get all products
        [HttpGet]
        [Route("api/product/all")]
        public HttpResponseMessage Get()
        {
            try
            {
                var data = ProductService.GetAllProducts();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Get a single product by ID
        [HttpGet]
        [Route("api/product/{id}")]
        public HttpResponseMessage GetProduct(int id)
        {
            try
            {
                var data = ProductService.GetProductById(id);
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Product not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Create a new product
        [HttpPost]
        [Route("api/product/create")]
        public HttpResponseMessage Create(ProductDTO data)
        {
            try
            {
                var result = ProductService.CreateProduct(data);
                if (result == "Product added successfully!")
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

        // Update an existing product
        [HttpPut]
        [Route("api/product/update/{id}")]
        public HttpResponseMessage Update(int id, ProductDTO data)
        {
            try
            {
                data.ProductId = id;
                var result = ProductService.UpdateProduct(data);
                if (result == "Product updated successfully!")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Product not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Delete a product
        [HttpDelete]
        [Route("api/product/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var result = ProductService.DeleteProduct(id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Product deleted successfully.");
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Product not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
