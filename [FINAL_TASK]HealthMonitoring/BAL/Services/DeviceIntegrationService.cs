using AutoMapper;
using BAL.DTOs;
using DAL;
using DAL.EF.Entities;
using System.Collections.Generic;

namespace BAL.Services
{
    public class DeviceIntegrationService
    {
        private static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeviceData, DeviceDataDTO>();
                cfg.CreateMap<DeviceDataDTO, DeviceData>();
            });
            return new Mapper(config);
        }

        public static bool SyncDeviceData(DeviceDataDTO deviceDataDto)
        {
            var data = GetMapper().Map<DeviceData>(deviceDataDto);
            return DataAccessFactory.DeviceIntegrationData().SyncDeviceData(data);
        }

        public static List<DeviceDataDTO> GetDeviceData(string username)
        {
            var data = DataAccessFactory.DeviceIntegrationData().GetDeviceData(username);
            return GetMapper().Map<List<DeviceDataDTO>>(data);
        }

        public static List<DeviceDataDTO> GetDeviceDataByPage(string username, int page, int pageSize)
        {
            var data = DataAccessFactory.DeviceIntegrationData().GetDeviceDataByPage(username, page, pageSize);
            return GetMapper().Map<List<DeviceDataDTO>>(data);
        }

        public static List<DeviceDataDTO> GetPaginatedDeviceData(string username, int page, int pageSize)
        {
            // Fetch paginated data from the repository
            var paginatedData = DataAccessFactory.DeviceIntegrationData().GetDeviceDataByPage(username, page, pageSize);

            // Map the entity data to DTO
            return GetMapper().Map<List<DeviceDataDTO>>(paginatedData);
        }

    }
}
