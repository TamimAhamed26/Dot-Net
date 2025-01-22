using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class User
    {
        [Key]
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string Username { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string Password { get; set; }

        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsVerified { get; set; } // New property

        // Navigation properties
        public virtual ICollection<Token> Tokens { get; set; } = new List<Token>(); //inline init.
        public virtual ICollection<HealthMetric> HealthMetrics { get; set; } = new List<HealthMetric>();
        public virtual ICollection<HealthGoal> HealthGoals { get; set; } = new List<HealthGoal>();
        public virtual ICollection<DeviceData> DeviceData { get; set; } = new List<DeviceData>();
        public virtual ICollection<SharedData> SharedData { get; set; } = new List<SharedData>();
    }
}
