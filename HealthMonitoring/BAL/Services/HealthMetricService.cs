using AutoMapper;
using BAL.DTOs;
using DAL.EF.Entities;
using DAL;
using System.Collections.Generic;
using System;

namespace BAL.Services
{
    public class HealthMetricService
    {
        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HealthMetric, HealthMetricDTO>();
                cfg.CreateMap<HealthMetricDTO, HealthMetric>();
            });
            return new Mapper(config);
        }


        public static bool Create(HealthMetricDTO obj)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName(); // Get the logged-in username
            if (obj.Username != loggedInUsername)
            {
                return false;
            }

            var data = GetMapper().Map<HealthMetric>(obj);
            return DataAccessFactory.HealthMetricData().Create(data);
        }


        public static List<HealthMetricDTO> Get()
        {
            var data = DataAccessFactory.HealthMetricData().Get();
            return GetMapper().Map<List<HealthMetricDTO>>(data);
        }
        public static HealthMetricDTO Get(int id)
        {
            var data = DataAccessFactory.HealthMetricData().Get(id);
            return GetMapper().Map<HealthMetricDTO>(data);
        }


        public static bool Update(HealthMetricDTO obj)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            if (obj.Username != loggedInUsername)
            {
                return false;
            }

            var data = GetMapper().Map<HealthMetric>(obj);
            return DataAccessFactory.HealthMetricData().Update(data);
        }


        public static bool Delete(int id)
        {
            return DataAccessFactory.HealthMetricData().Delete(id);
        }
        // HealthMetric Features

        // Get metrics by date range (
        public static List<HealthMetricDTO> GetMetricsByDateRange(string username, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName(); // Get the logged-in username
            if (username != loggedInUsername)
            {
                return new List<HealthMetricDTO>();
            }

            var data = DataAccessFactory.HealthMetricFeatures().GetMetricsByDateRange(username, startDate, endDate);
            return GetMapper().Map<List<HealthMetricDTO>>(data);
        }

        // Get average metric value
        public static double GetAverageMetricValue(string username, string metricType, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName(); // Get the logged-in username
            if (username != loggedInUsername)
            {
                return 0;
            }

            return DataAccessFactory.HealthMetricFeatures().GetAverageMetricValue(username, metricType, startDate, endDate);
        }

        // Get min/max metric values 
        public static (double minValue, double maxValue) GetMinMaxMetricValues(string username, string metricType, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName(); // Get the logged-in username
            if (username != loggedInUsername)
            {
                return (0, 0);
            }

            return DataAccessFactory.HealthMetricFeatures().GetMinMaxMetricValues(username, metricType, startDate, endDate);
        }

        // Get metric trends 
        public static Dictionary<string, int> GetMetricTrends(string username, DateTime startDate, DateTime endDate)
        {
            var loggedInUsername = AuthService.GetLoggedInUserName();
            if (username != loggedInUsername)
            {
                return new Dictionary<string, int>();
            }

            return DataAccessFactory.HealthMetricFeatures().GetMetricTrends(username, startDate, endDate);
        }


        public static List<(DateTime Date, double Value)> GetMetricHistory(string username, string metricType, DateTime startDate, DateTime endDate)
        {
            return DataAccessFactory.HealthMetricFeatures()
                .GetMetricHistory(username, metricType, startDate, endDate);
        }

    }
}
