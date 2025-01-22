using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IHealthGoalFeatures
    {
        bool CreateHealthGoal(HealthGoal goal);
        List<HealthGoal> GetHealthGoals(string username);
        bool UpdateGoalStatus(int goalId, string status);
        void UpdateProgressBasedOnMetrics(string username);
        List<HealthGoal> Get();
        HealthGoal Get(int id); 
        bool Delete(int id); 
    }

}
