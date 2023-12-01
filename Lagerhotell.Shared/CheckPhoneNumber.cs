using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models
{
    public class CheckPhoneNumberRequest
    {
        public string PhoneNumber { get; set;}
    }
    public class CheckPhoneNumberResponse {  public bool PhoneNumberExistence { get; set;} }
}
