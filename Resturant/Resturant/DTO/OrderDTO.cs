using System;
using System.ComponentModel.DataAnnotations;

namespace Resturant.DTO
{
    public class OrderDTO
    {
        [Required(ErrorMessage = "ItemId is required.")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "CustomerName is required.")]
        [StringLength(100, ErrorMessage = "CustomerName must not exceed 100 characters.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "OrderDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? OrderDate { get; set; }
    }
}
