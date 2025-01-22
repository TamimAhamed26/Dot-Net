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
    public class CustomerController : ApiController
    {
        // Get all customers
        [HttpGet]
        [Route("api/customer/all")]
        public HttpResponseMessage Get()
        {
            try
            {
                var data = CustomerService.GetAllCustomers();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Get a single customer by ID
        [HttpGet]
        [Route("api/customer/{id}")]
        public HttpResponseMessage GetCustomer(int id)
        {
            try
            {
                var data = CustomerService.GetCustomerById(id);
                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Create a new customer
        [HttpPost]
        [Route("api/customer/create")]
        public HttpResponseMessage Create(CustomerDTO data)
        {
            try
            {
                var result = CustomerService.CreateCustomer(data);
                if (result == "Customer added successfully!")
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
        [Route("api/customer/update/{id}")]
        public HttpResponseMessage Update(int id, CustomerDTO data)
        {
            try
            {
                data.CustomerId = id;
                var result = CustomerService.UpdateCustomer(data);
                if (result == "Customer updated successfully!")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        // Delete a customer
        [HttpDelete]
        [Route("api/customer/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var result = CustomerService.DeleteCustomer(id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Customer deleted successfully.");
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
