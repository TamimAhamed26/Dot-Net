using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BAL.DTOs
{
    public class CustomerDTO
    {
        [Required]
        public int CustomerId { get; set; } // Primary Key

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Customer name must be between 3 and 50 characters.")]
        public string Name { get; set; } // Customer Name

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Customer Email

      
    }
}
