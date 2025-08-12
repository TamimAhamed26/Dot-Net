using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PictureU.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [StringLength(25, ErrorMessage = "Name cannot be longer than 25 characters.")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Path cannot be longer than 200 characters.")]
        public string? PicturePath { get; set; }

        [NotMapped]
        public IFormFile? Picture { get; set; }


    }
}
