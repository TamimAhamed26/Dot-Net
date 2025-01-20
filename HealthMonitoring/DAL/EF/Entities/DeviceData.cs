using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Entities
{
    public class DeviceData
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string Username { get; set; }
        public string DeviceName { get; set; } 
        public string MetricType { get; set; } 
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }


        public virtual User User { get; set; }
    }

}
