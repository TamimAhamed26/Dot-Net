using System;
using System.ComponentModel.DataAnnotations;

namespace Hotel.DTO
{
    public class BookingDTO
    {
        [Key]
        public int BookingID { get; set; }

        [Required(ErrorMessage = "Room Number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Room Number.")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Customer Name is required.")]
        [StringLength(100, ErrorMessage = "Customer Name can't exceed 100 characters.")]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Check-in date must be at least one day in the future.")]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }



    }

   
}
