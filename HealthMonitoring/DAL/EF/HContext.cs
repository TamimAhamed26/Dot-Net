using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class HContext : DbContext
    {
        public DbSet<SharedData> SharedDatas { get; set; }
        public DbSet<HealthMetric> HealthMetrics { get; set; }
        public DbSet<HealthGoal> HealthGoals { get; set; }
        public DbSet<DeviceData> DeviceDatas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}
