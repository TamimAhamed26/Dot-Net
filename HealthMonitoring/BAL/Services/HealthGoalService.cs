using AutoMapper;
using BAL.DTOs;
using DAL.EF.Entities;
using DAL;
using System.Collections.Generic;
using System.Linq;

namespace BAL.Services
{
    public class HealthGoalService
    {
        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HealthGoal, HealthGoalDTO>();
                cfg.CreateMap<HealthGoalDTO, HealthGoal>();
            });
            return new Mapper(config);
        }

        public static bool Create(HealthGoalDTO obj)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName(); // Get the logged-in username
            if (obj.Username != loggedInUsername)
            {
                return false;
            }

            var data = GetMapper().Map<HealthGoal>(obj);
            return DataAccessFactory.HealthGoalFeatures().CreateHealthGoal(data);
        }

        public static List<HealthGoalDTO> Get()
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            var data = DataAccessFactory.HealthGoalFeatures().GetHealthGoals(loggedInUsername);
            return GetMapper().Map<List<HealthGoalDTO>>(data);
        }

        public static HealthGoalDTO Get(int id)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            var goals = DataAccessFactory.HealthGoalFeatures().GetHealthGoals(loggedInUsername);
            var goal = goals.FirstOrDefault(g => g.Id == id);

            return goal != null ? GetMapper().Map<HealthGoalDTO>(goal) : null;
        }

        public static bool Update(HealthGoalDTO obj)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            if (obj.Username != loggedInUsername)
            {
                return false;
            }

            var data = GetMapper().Map<HealthGoal>(obj);
            return DataAccessFactory.HealthGoalFeatures().CreateHealthGoal(data); ;
        }

        public static bool Delete(int id)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            var goals = DataAccessFactory.HealthGoalFeatures().GetHealthGoals(loggedInUsername);
            var goal = goals.FirstOrDefault(g => g.Id == id);

            if (goal != null)
            {
                goal.Status = "Deleted";
                return DataAccessFactory.HealthGoalFeatures().UpdateGoalStatus(goal.Id, goal.Status);
            }

            return false;
        }

        // Update progress based on health metrics
        public static void UpdateProgress(string username)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            if (username == loggedInUsername)
            {
                DataAccessFactory.HealthGoalFeatures().UpdateProgressBasedOnMetrics(username);
            }
        }

        public static List<HealthGoalDTO> GetAllByUser(string username)
        {
            var allGoals = DataAccessFactory.HealthGoalFeatures().Get();
            return allGoals.Where(g => g.Username == username).Select(g => GetMapper().Map<HealthGoalDTO>(g)).ToList();
        }

        public static HealthGoalDTO GetByUserAndId(string username, int id)
        {
            var goal = DataAccessFactory.HealthGoalFeatures().Get(id);
            if (goal != null && goal.Username == username)
            {
                return GetMapper().Map<HealthGoalDTO>(goal);
            }
            return null;
        }

        public static bool DeleteByUserAndId(string username, int id)
        {
            var goal = DataAccessFactory.HealthGoalFeatures().Get(id);
            if (goal != null && goal.Username == username)
            {
                return DataAccessFactory.HealthGoalFeatures().Delete(id);
            }
            return false;
        }
    }
}
