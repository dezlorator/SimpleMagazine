using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _3Lab.Auth
{
    public class JwtTokenCryptographer : IJwtTokenCryptographer
    {
        private readonly JwtSecurityTokenHandler _JwtSecurityTokenHandler;

        public JwtTokenCryptographer(JwtSecurityTokenHandler JwtSecurityTokenHandler)
        {
            _JwtSecurityTokenHandler = JwtSecurityTokenHandler;
        }

        public string Encode(ClaimsIdentity claimsIdentity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                signingCredentials: new Microsoft.IdentityModel.Tokens
                    .SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
                );
           return _JwtSecurityTokenHandler.WriteToken(jwt);
        }
    }
}
