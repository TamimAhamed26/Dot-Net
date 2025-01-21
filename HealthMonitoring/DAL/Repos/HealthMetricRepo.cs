using DAL.EF.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    internal class HealthMetricRepo : Repo, IRepo<HealthMetric, int, bool>, IHealthMetricFeatures
    {
        public bool Create(HealthMetric obj)
        {
            db.HealthMetrics.Add(obj);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exobj = Get(id);
            if (exobj != null)
            {
                db.HealthMetrics.Remove(exobj);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public List<HealthMetric> Get()
        {
            return db.HealthMetrics.ToList();
        }

        public HealthMetric Get(int id)
        {
            return db.HealthMetrics.Find(id);
        }

        public bool Update(HealthMetric obj)
        {
            var exobj = Get(obj.Id);
            if (exobj != null)
            {
                db.Entry(exobj).CurrentValues.SetValues(obj);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public List<HealthMetric> GetMetricsByDateRange(string username, DateTime startDate, DateTime endDate)
        {
            return db.HealthMetrics
                .Where(hm => hm.Username == username && hm.Date >= startDate && hm.Date <= endDate)
                .ToList();
        }

        public double GetAverageMetricValue(string username, string metricType, DateTime startDate, DateTime endDate)
        {
            return db.HealthMetrics
                .Where(hm => hm.Username == username && hm.MetricType == metricType && hm.Date >= startDate && hm.Date <= endDate)
                .Select(hm => hm.Value)
                .DefaultIfEmpty(0)
                .Average();
        }

        public (double minValue, double maxValue) GetMinMaxMetricValues(string username, string metricType, DateTime startDate, DateTime endDate)
        {
            var metrics = db.HealthMetrics
                .Where(hm => hm.Username == username && hm.MetricType == metricType && hm.Date >= startDate && hm.Date <= endDate)
                .Select(hm => hm.Value);

            return metrics.Any()
                ? (metrics.Min(), metrics.Max())
                : (0, 0);
        }

        public Dictionary<string, int> GetMetricTrends(string username, DateTime startDate, DateTime endDate)
        {
            return db.HealthMetrics
                .Where(hm => hm.Username == username && hm.Date >= startDate && hm.Date <= endDate)
                .GroupBy(hm => hm.MetricType)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<(DateTime Date, double Value)> GetMetricHistory(string username, string metricType, DateTime startDate, DateTime endDate)
        {
            return db.HealthMetrics
                .Where(hm => hm.Username == username && hm.MetricType == metricType && hm.Date >= startDate && hm.Date <= endDate)
                .OrderBy(hm => hm.Date) // Order by date 
                .Select(hm => new { hm.Date, hm.Value })
                .AsEnumerable()
                .Select(hm => (hm.Date, hm.Value))
                .ToList();

        }
    }
}