using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _3Lab.Services
{
    public interface IGetClaims
    {
        Task<ClaimsIdentity> Claims(string userName, string password);
    }
}
