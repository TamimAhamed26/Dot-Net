using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http; // Needed for IFormFile

namespace StudentCRUD.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        [AllowNull]
        public string? Address { get; set; }

        public string? PicturePath { get; set; }

        [NotMapped]
        public IFormFile? PictureFile { get; set; }
    }
}
