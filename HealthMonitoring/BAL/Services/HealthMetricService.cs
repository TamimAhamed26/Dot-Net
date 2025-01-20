using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class HealthMetricService
    {
        static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<HealthMetric, HealthMetricDTO>();
                cfg.CreateMap<HealthMetricDTO, HealthMetric>();
            });
            return new Mapper(config);
        }
        public static bool Create(HealthMetricDTO obj)
        {
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
            var data = GetMapper().Map<HealthMetric>(obj);
            return DataAccessFactory.HealthMetricData().Update(data);
        }
        public static bool Delete(int id)
        {
            return DataAccessFactory.HealthMetricData().Delete(id);
        }
    }
}
