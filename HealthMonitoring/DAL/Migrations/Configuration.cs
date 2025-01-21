namespace DAL.Migrations
{
    using DAL.EF.Entities;
    using DAL.EF;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.EF.HContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HContext context)
        {
            // Seed Users with verification
            var user1 = new User
            {
                Username = "tamim",
                Password = "123456",
                Email = "tamim26mar@gmai.com",
                Role = "Admin",
                IsVerified = true // Set verification to true for admin
            };

            var user2 = new User
            {
                Username = "user1",
                Password = "654321",
                Email = "user@gmail.com.com",
                Role = "User",
                IsVerified = true // Set verification to true for existing user
            };

            context.Users.AddOrUpdate(u => u.Username, user1, user2);

            // Seed Health Metrics
            var healthMetric1 = new HealthMetric
            {
                Username = "user1",
                MetricType = "Weight",
                Value = 70.5,
                Date = new DateTime(2025, 1, 20)
            };

            var healthMetric2 = new HealthMetric
            {
                Username = "user1",
                MetricType = "Blood Pressure",
                Value = 120.80,
                Date = new DateTime(2025, 1, 19)
            };

            context.HealthMetrics.AddOrUpdate(h => h.Id, healthMetric1, healthMetric2);

            // Seed Health Goals
            context.HealthGoals.AddOrUpdate(
                g => g.Id, 
                new HealthGoal
                {
                    Username = "user1",
                    GoalType = "Weight Loss",
                    TargetValue = 65,
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 6, 1),
                    Status = "In Progress"
                },
                new HealthGoal
                {
                    Username = "user1",
                    GoalType = "Exercise",
                    TargetValue = 30, // Target minutes per day
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2025, 12, 31),
                    Status = "Active"
                },
                   new HealthGoal
                   {
                       Username = "jane",
                       GoalType = "Exercise",
                       TargetValue = 40, // Target minutes per day
                       StartDate = new DateTime(2024, 1, 1),
                       EndDate = new DateTime(2025, 02, 28),
                       Status = "In Progress"
                   },
                    new HealthGoal
                    {
                        Username = "jane",
                        GoalType = "Weight Loss",
                        TargetValue = 40, // Target minutes per day
                        StartDate = new DateTime(2024, 1, 1),
                        EndDate = new DateTime(2026, 12, 31),
                        Status = "In Progress"
                    }



            );

            // Seed Device Data
            context.DeviceDatas.AddOrUpdate(
                d => d.Id, // Avoid duplicates based on Id
                new DeviceData
                {
                    Username = "user1",
                    DeviceName = "Fitbit 5",
                    MetricType = "Steps",
                    Value = 5000,
                    Timestamp = new DateTime(2025, 1, 19, 9, 0, 0)
                },
                new DeviceData
                {
                    Username = "user1",
                    DeviceName = "Mi Band 6",
                    MetricType = "Heart Rate",
                    Value = 75,
                    Timestamp = new DateTime(2025, 1, 19, 10, 0, 0)
                }
            );

            context.SaveChanges();
        }
    }
}
