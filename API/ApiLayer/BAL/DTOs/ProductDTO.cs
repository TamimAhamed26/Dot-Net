
using System.ComponentModel.DataAnnotations;

namespace BAL.DTOs
{
    public class ProductDTO
    {
        [Required]
        public int ProductId { get; set; } // Primary Key

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters.")]
        public string Name { get; set; } // Product Name

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000.00, ErrorMessage = "Price must be between 0.01 and 100,000.")]
        public decimal Price { get; set; } // Product Price

        // Foreign Key
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }

        // Navigation Property (optional for hierarchical data transfer)
        public CustomerDTO Customer { get; set; }
    }
}
