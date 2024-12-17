using System;
using System.ComponentModel.DataAnnotations;

namespace labexam.DTO
{
    public class UserDTO
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 255 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("Admin|User", ErrorMessage = "Role must be either 'Admin' or 'User'.")]
        public string Role { get; set; }
    }
}
