using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LabPrac.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }


        
        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must be between 10 and 15 digits")]
        public string Phone { get; set; }

        
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Date joined is required")]
        [DataType(DataType.Date)]
        [DateJoinedValidation(ErrorMessage = "Date joined cannot be in the future")]
        public DateTime DateJoined { get; set; }
    }
}