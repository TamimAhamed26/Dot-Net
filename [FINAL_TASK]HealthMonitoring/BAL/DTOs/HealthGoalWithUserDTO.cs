using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class HealthGoalWithUserDTO :UserDTO
    {
        public List<HealthGoalDTO> HealthGoals { get; set; }

        public HealthGoalWithUserDTO()
        {
            HealthGoals = new List<HealthGoalDTO>();
        }
    }
}
