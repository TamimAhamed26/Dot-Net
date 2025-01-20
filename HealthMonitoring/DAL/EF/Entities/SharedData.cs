using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class SharedData
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string Username { get; set; }
        public string SharedWith { get; set; } //  healthcare provider's email
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
    
        public virtual User User { get; set; }
    }
}
