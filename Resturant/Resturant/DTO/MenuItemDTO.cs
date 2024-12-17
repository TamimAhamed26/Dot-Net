using System;
using System.ComponentModel.DataAnnotations;

namespace Resturant.DTO
{
    public class MenuItemDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Availability is required.")]
        public bool? Availability { get; set; }

        [Required(ErrorMessage = "MenuId is required.")]
        public int MenuId { get; set; }

        [Required(ErrorMessage = "ItemId is Required.")]
        public int ItemId { get; set; }

    }
}