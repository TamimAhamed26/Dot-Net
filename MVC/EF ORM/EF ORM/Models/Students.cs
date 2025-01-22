using System;
using System.ComponentModel.DataAnnotations;

namespace MvcEFApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
    }
}
