using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class HealthGoalDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string GoalType { get; set; }
        public double TargetValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
