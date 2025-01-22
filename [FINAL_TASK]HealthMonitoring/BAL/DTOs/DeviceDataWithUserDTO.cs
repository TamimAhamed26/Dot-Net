using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class DeviceDataWithUserDTO : UserDTO
    {
        public List<DeviceDataDTO> DeviceDatas { get; set; }

        public DeviceDataWithUserDTO()
        {
            DeviceDatas = new List<DeviceDataDTO>();
        }
    }
}

