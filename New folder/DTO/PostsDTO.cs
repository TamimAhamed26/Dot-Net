using System;
using System.ComponentModel.DataAnnotations;

namespace labexam.DTO
{
    public class PostsDTO
    {
        public int PostID { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public Nullable<System.DateTime> Date { get; set; }

        [Required(ErrorMessage = "UserID is required.")]
        public int UserID { get; set; }
    }
}
