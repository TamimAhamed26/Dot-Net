using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class DeviceDataDTO
    {
        public int Id { get; set; }

   
        public string Username { get; set; }
        public string DeviceName { get; set; }
        public string MetricType { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
