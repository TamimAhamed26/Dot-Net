using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsVerified = false; // Set verification to false
    }
}
