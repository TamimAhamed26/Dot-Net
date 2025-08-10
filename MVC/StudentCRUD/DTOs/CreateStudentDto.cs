using System.ComponentModel.DataAnnotations;

namespace StudentCRUD.DTOs
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must only contain letters and spaces")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
        public int? Age { get; set; }  // nullable int to allow validation to kick in

        [StringLength(100, ErrorMessage = "Address can't exceed 100 characters")]
        public string? Address { get; set; }  // nullable string to make it optional

        public IFormFile? PictureFile { get; set; }

    }
}
