using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class TokenWithUserDTO : UserDTO
    {
        public List<TokenDTO> Tokens { get; set; }

        public TokenWithUserDTO()
        {
            Tokens = new List<TokenDTO>();
        }
    }
}
