using DAL.EF.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    internal class HealthGoalRepo : Repo, IRepo<HealthGoal, int, bool>, IHealthGoalFeatures
    {
        public bool CreateHealthGoal(HealthGoal goal)
        {
            db.HealthGoals.Add(goal);
            return db.SaveChanges() > 0;
        }

        public List<HealthGoal> GetHealthGoals(string username)
        {
            return db.HealthGoals.Where(hg => hg.Username == username).ToList();
        }

        public bool UpdateGoalStatus(int goalId, string status)
        {
            var goal = db.HealthGoals.Find(goalId);
            if (goal != null)
            {
                goal.Status = status;
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public void UpdateProgressBasedOnMetrics(string username)
        {
            var userGoals = db.HealthGoals.Where(hg => hg.Username == username).ToList();
            var userMetrics = db.HealthMetrics.Where(hm => hm.Username == username).ToList();

            foreach (var goal in userGoals)
            {
             
                var relevantMetrics = userMetrics
                    .Where(hm => hm.MetricType == goal.GoalType && hm.Date >= goal.StartDate && hm.Date <= goal.EndDate)
                    .ToList();

                if (relevantMetrics.Any())
                {
                   
                    var progressValue = relevantMetrics.Average(hm => hm.Value);
                    var progressPercentage = (progressValue / goal.TargetValue) * 100;

                    
                    if (progressPercentage >= 100)
                    {
                        goal.Status = "Achieved";
                    }
                    else if (progressPercentage > 0)
                    {
                        goal.Status = $"In Progress ({progressPercentage:F2}%)";
                    }
                    else
                    {
                        goal.Status = "Not Started";
                    }
                }
                else
                {
                    goal.Status = "No Metrics Available";
                }
            }

            db.SaveChanges();
        }


        public List<HealthGoal> Get()
        {
            return db.HealthGoals.ToList(); 
        }

        public HealthGoal Get(int id)
        {
            return db.HealthGoals.Find(id);
        }

        public bool Delete(int id)
        {
            var goal = db.HealthGoals.Find(id);
            if (goal != null)
            {
                db.HealthGoals.Remove(goal);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public bool Create(HealthGoal obj)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(HealthGoal obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
