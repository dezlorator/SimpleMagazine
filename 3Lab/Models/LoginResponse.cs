using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3Lab.Models
{
    public class LoginResponse
    {
        public string access_token { get; set; }
        public string username { get; set; }
        public UserRole Role { get; set; }
    }
}
