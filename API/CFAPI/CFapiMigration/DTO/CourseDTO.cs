using System.ComponentModel.DataAnnotations;

namespace CFapiMigration.DTO
{
    public class CourseDTO
    {
        [Key]
        public int Id { get; set; } // Primary key
        [Required(ErrorMessage = "Title is required")]
        [StringLength(12, ErrorMessage = "Name must be between 2 and 12 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Credit is required")]
        [Range(1, 4, ErrorMessage = "Credits must be 1, 3, or 4")]
        public int Credits { get; set; }
    }
}