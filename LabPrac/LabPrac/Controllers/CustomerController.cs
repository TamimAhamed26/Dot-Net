using LabPrac.DTO;
using LabPrac.EF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LabPrac.Controllers
{
    public class CustomerController : Controller
    {
        private readonly LabEntities4 db = new LabEntities4();

        // GET: Customer/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FirstName = customerDTO.FirstName,
                    LastName = customerDTO.LastName,
                    Email = customerDTO.Email,
                    Phone = customerDTO.Phone,
                    Address = customerDTO.Address,
                    DateJoined = customerDTO.DateJoined
                };
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(customerDTO);
        }

        // GET: Customer/List
        public ActionResult List()
        {
            var customers = db.Customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                DateJoined = c.DateJoined
            }).ToList();

            return View(customers);
        }

        // GET: Customer/Details/{id}
        public ActionResult Details(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                TempData["Msg"] = "Customer with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var customerDTO = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                DateJoined = customer.DateJoined
            };

            return View(customerDTO);
        }

    
        // GET: Customer/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();

            var customerDTO = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                DateJoined = customer.DateJoined
            };

            return View(customerDTO);
        }

        // POST: Customer/Edit
        [HttpPost]
        public ActionResult Edit(CustomerDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                var customer = db.Customers.Find(customerDTO.CustomerId);
                if (customer == null) return HttpNotFound();

                db.Entry(customer).CurrentValues.SetValues(customerDTO);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(customerDTO);
        }
       

        // GET: Customer/Delete/{id}
        public ActionResult Delete(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Check if the customer has related orders
            bool hasOrders = db.Orders.Any(o => o.CustomerId == id);
            if (hasOrders)
            {
                
                var customerDTO = new CustomerDTO
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email
                };

                ViewBag.HasOrders = true; 
                return View("ConfirmDelete", customerDTO);
            }

            
            db.Customers.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        // POST: DeleteConfirmed
        [HttpPost]
        public ActionResult DeleteConfirmed(int customerId)
        {
            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Delete related orders first
            var relatedOrders = db.Orders.Where(o => o.CustomerId == customerId).ToList();
            db.Orders.RemoveRange(relatedOrders);

            // Delete the customer
            db.Customers.Remove(customer);
            db.SaveChanges();

            TempData["Success"] = "Customer and related orders deleted successfully.";
            return RedirectToAction("List");
        }


    }
}
