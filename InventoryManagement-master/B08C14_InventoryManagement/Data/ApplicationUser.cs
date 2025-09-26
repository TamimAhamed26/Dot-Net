using Microsoft.AspNetCore.Identity;

namespace B08C14_InventoryManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? PhoneNo { get; set; }
    }
}
