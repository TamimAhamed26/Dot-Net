using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.DTO
{
    public class RoomDTO
    {
        [Key]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Room Type is required.")]
        [StringLength(50, ErrorMessage = "Room Type can't exceed 50 characters.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Room Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Room Availability is required.")]
        public bool Availability { get; set; }
    }
}