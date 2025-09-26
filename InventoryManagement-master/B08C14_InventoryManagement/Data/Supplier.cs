using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace B08C14_InventoryManagement.Data
{
    public class Supplier:BaseEntity
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Address { get; set; } = "";
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser? User { get; set; }
    }
}