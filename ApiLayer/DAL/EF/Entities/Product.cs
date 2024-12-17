using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.EF.Entities
{
    public class Product
    {
        public int ProductId { get; set; } // Primary Key
        public string Name { get; set; } // Product Name
        public decimal Price { get; set; } // Product Price

        // Foreign Key
        public int? CustomerId { get; set; } // Nullable if not mandatory

        // Navigation Property
        public virtual Customer Customer { get; set; }
    }
}
