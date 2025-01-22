using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class SharedDataWithUserDTO : UserDTO
    {
        public List<SharedDataDTO> SharedDatas { get; set; }

        public SharedDataWithUserDTO()
        {
            SharedDatas = new List<SharedDataDTO>();
        }
    }
}
