using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class HealthGoal
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string Username { get; set; }
        public string GoalType { get; set; } 
        public double TargetValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

   
        public virtual User User { get; set; }
    }

}
