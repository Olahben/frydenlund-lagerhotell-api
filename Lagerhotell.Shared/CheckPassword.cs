using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagerhotell.Shared
{
    public class CheckPassword
    {
        public class CheckPasswordRequest
        {
            public string PhoneNumber { get; set; }
        }

        public class CheckPasswordResponse
        {
            public string Password { get; set; }
        }
    }
}
