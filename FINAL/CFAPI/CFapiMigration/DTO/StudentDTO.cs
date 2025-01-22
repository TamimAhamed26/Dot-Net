using System.ComponentModel.DataAnnotations;

namespace CFapiMigration.DTO
{
    public class StudentDTO
    {

        //dto for avoiding JSON serialization & circular references

        [Key]
        public int Id { get; set; } // Primary key

        [Required(ErrorMessage = "Name is required")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 12 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 50, ErrorMessage = "Age must be between 1 and 50")]
        public int Age { get; set; }



    }
}