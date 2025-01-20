using DAL.EF.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    internal class HealthMetricRepo : Repo, IRepo<HealthMetric, int, bool>
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
    }
}
