using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _3Lab.Auth
{
    public interface IJwtTokenCryptographer
    {
        string Encode(ClaimsIdentity claimsIdentity);
    }
}
