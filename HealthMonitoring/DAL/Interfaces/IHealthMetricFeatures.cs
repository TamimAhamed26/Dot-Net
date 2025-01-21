using DAL.EF.Entities;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IHealthMetricFeatures
    {
        // within a date range for a specific user
        List<HealthMetric> GetMetricsByDateRange(string username, DateTime startDate, DateTime endDate);

        double GetAverageMetricValue(string username, string metricType, DateTime startDate, DateTime endDate);

        (double minValue, double maxValue) GetMinMaxMetricValues(string username, string metricType, DateTime startDate, DateTime endDate);

        // Count of occurrences of each metric type for trends
        Dictionary<string, int> GetMetricTrends(string username, DateTime startDate, DateTime endDate);

        List<(DateTime Date, double Value)> GetMetricHistory(string username, string metricType, DateTime startDate, DateTime endDate);

    }
}
