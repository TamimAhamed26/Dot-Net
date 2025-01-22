using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class HealthMetricWithUserDTO : UserDTO
    {
        public List<HealthMetricDTO> HealthMetrics { get; set; }

        public HealthMetricWithUserDTO()
        {
            HealthMetrics = new List<HealthMetricDTO>();
        }
    }
}
