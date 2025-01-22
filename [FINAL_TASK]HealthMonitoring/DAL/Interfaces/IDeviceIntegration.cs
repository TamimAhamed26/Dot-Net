using DAL.EF.Entities;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IDeviceIntegration
    {
        bool SyncDeviceData(DeviceData deviceData);
        List<DeviceData> GetDeviceData(string username);
        List<DeviceData> GetDeviceDataByPage(string username, int page, int pageSize);
       

    }
}
