using LabPrac.DTO;
using LabPrac.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabPrac.Controllers
{
    public class ProductController : Controller
    {
        private readonly LabEntities4 db = new LabEntities4();

      
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    StockQuantity = productDTO.StockQuantity,
                    Category = productDTO.Category
                };
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(productDTO);
        }

        // GET: Product/List
        public ActionResult List()
        {
            var products = db.Products.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Category = p.Category
            }).ToList();

            return View(products);
        }

        // GET: Product/Details/{id}
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                TempData["Msg"] = "Product with ID " + id + " not found";
                return RedirectToAction("List");
            }

            var productDTO = new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Category = product.Category
            };

            return View(productDTO);
        }

        // GET: Product/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            var productDTO = new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Category = product.Category
            };

            return View(productDTO);
        }

        // POST: Product/Edit
        [HttpPost]
        public ActionResult Edit(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                var product = db.Products.Find(productDTO.ProductId);
                if (product == null) return HttpNotFound();

                db.Entry(product).CurrentValues.SetValues(productDTO);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(productDTO);
        }

        public ActionResult Delete(int id, bool confirm = false)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("List");
            }

            var relatedOrderItems = db.OrderItems.Where(oi => oi.ProductId == id).ToList();
            if (relatedOrderItems.Any())
            {
                if (!confirm)
                {
                   
                    ViewBag.RelatedOrderItems = relatedOrderItems;
                    return View("ConfirmDelete", product);
                }

               
                db.OrderItems.RemoveRange(relatedOrderItems);
            }

            // Delete the product
            db.Products.Remove(product);
            db.SaveChanges();

            TempData["Success"] = "Product and related order items were successfully deleted.";
            return RedirectToAction("List");
        }

    }
}
