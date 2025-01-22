using DAL.EF.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class ProductRepo : Repo, IRepo<Product, int, string>
    {
        public List<Product> Get()
        {
            return db.Products.ToList();
        }

        public Product Get(int id)
        {
            return db.Products.Find(id);
        }

        public string Create(Product obj)
        {
            try
            {
                db.Products.Add(obj);
                db.SaveChanges();
                return "Product added successfully!";
            }
            catch
            {
                return "Error occurred while adding product.";
            }
        }

        public string Update(Product obj)
        {
            try
            {
                var existing = db.Products.Find(obj.ProductId);
                if (existing != null)
                {
                    db.Entry(existing).CurrentValues.SetValues(obj);
                    db.SaveChanges();
                    return "Product updated successfully!";
                }
                return "Product not found.";
            }
            catch
            {
                return "Error occurred while updating product.";
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
