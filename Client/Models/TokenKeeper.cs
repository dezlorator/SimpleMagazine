using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public static class TokenKeeper
    {
        public static string Token { get; set; }

        static TokenKeeper()
        {
            Token = String.Empty;
        }
    }
}
