using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LabPrac.DTO
{
    public class ProductDTO
    {
      
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }


        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater")]
        public int StockQuantity { get; set; }


        [Required(ErrorMessage = "Category is required")]
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
        [CategoryValidation(ErrorMessage = "Invalid category")]
        public string Category { get; set; }
       
 
    }
}