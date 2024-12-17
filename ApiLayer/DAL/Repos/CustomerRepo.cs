using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.EF;
namespace DAL.Repos
{
    public class CustomerRepo : Repo, IRepo<Customer, int, string>
    {
        public List<Customer> Get()
        {
            return db.Customers.ToList();
        }

        public Customer Get(int id)
        {
            return db.Customers.Find(id);
        }

        public string Create(Customer obj)
        {
            try
            {
                db.Customers.Add(obj);
                db.SaveChanges();
                return "Customer added successfully!";
            }
            catch
            {
                return "Error occurred while adding customer.";
            }
        }

        public string Update(Customer obj)
        {
            try
            {
                var existing = db.Customers.Find(obj.CustomerId);
                if (existing != null)
                {
                    db.Entry(existing).CurrentValues.SetValues(obj);
                    db.SaveChanges();
                    return "Customer updated successfully!";
                }
                return "Customer not found.";
            }
            catch
            {
                return "Error occurred while updating customer.";
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var customer = db.Customers.Find(id);
                if (customer != null)
                {
                    db.Customers.Remove(customer);
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
