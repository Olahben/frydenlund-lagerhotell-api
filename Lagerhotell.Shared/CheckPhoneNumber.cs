using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LagerhotellAPI.Models
{
    public class CheckPhoneNumber
    {
        public class CheckPhoneNumberRequest
        {
            public string PhoneNumber { get; set; }
        }

        public class CheckPhoneNumberResponse { public bool PhoneNumberExistence { get; set; } }
    }
}
