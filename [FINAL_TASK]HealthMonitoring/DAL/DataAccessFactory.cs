using DAL.EF.Entities;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataAccessFactory
    {
        public static IRepo<HealthMetric, int, bool> HealthMetricData()
        {
            return new HealthMetricRepo();
        }
        public static IRepo<HealthGoal, int, bool> HealthGoalData()
        {
            return new HealthGoalRepo();
        }
        public static IRepo<User, string, User> UserData()
        {
            return new UserRepo();
        }
        public static IAuth AuthData()
        {
            return new UserRepo();
        }
        public static IRepo<Token, string, Token> TokenData()
        {
            return new TokenRepo();
        }

        public static IHealthMetricFeatures HealthMetricFeatures()
        {
            return new HealthMetricRepo();
        }
        public static IHealthGoalFeatures HealthGoalFeatures()
        {
            return new HealthGoalRepo();
        }
        public static IDeviceIntegration DeviceIntegrationData()
        {
            return new DeviceIntegrationRepo();
        }
        public static ISharedDataFeatures SharedDataFeatures()
        {
            return new SharedDataRepo();
        }

    }
}