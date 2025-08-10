using System.ComponentModel.DataAnnotations;

namespace StudentCRUD.DTOs
{
    public class EditStudentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Range(1, 150)]
        public int Age { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        public IFormFile? PictureFile { get; set; }

        public string? ExistingPicturePath { get; set; }

    }
}
