using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class HealthMetricDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string MetricType { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }

     
    }
}
