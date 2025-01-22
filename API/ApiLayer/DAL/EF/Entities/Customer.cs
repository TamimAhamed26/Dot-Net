using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; } // Primary Key
        public string Name { get; set; } // Customer Name
        public string Email { get; set; } // Customer Email

        // Navigation Property
        public virtual ICollection<Product> Products { get; set; }

        public Customer()
        {
            Products = new List<Product>();
        }

    }
}
