using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class SharedDataDTO
    {
        public int Id { get; set; }

    
        public string Username { get; set; }
        public string SharedWith { get; set; } //  healthcare provider's email
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
    }
}
