using DAL.EF.Entities;
using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    internal class DeviceIntegrationRepo : Repo, IDeviceIntegration
    {
        public bool SyncDeviceData(DeviceData deviceData)
        {
            db.DeviceDatas.Add(deviceData);
            return db.SaveChanges() > 0;
        }

        public List<DeviceData> GetDeviceData(string username)
        {
            return db.DeviceDatas.Where(d => d.Username == username).ToList();
        }
        public List<DeviceData> GetDeviceDataByPage(string username, int page, int pageSize)
        {
            return db.DeviceDatas
                .Where(d => d.Username == username)
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
