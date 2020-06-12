using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public static class TokenKeeper
    {
        public static string Token { get; set; }
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static UserRole Permissions { get; set; }

        static TokenKeeper()
        {
            Token = String.Empty;
            UserName = String.Empty;
        }
    }
}
