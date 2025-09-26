using B08C14_InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace B08C14_InventoryManagement.Data
{
    public class Customer:BaseEntity
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public  string Mobile { get; set; } = "";
        public string Address { get; set; } = "";
      
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser? User { get; set; }
    }
}