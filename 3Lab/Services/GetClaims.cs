using _3Lab.Auth;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _3Lab.Services
{
    public class GetClaims : IGetClaims
    {
        private ApplicationDbContext _context;
        private IPasswordHasher _passwordHasher;

        public GetClaims(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public Task<ClaimsIdentity> Claims(string userName, string password)
        {
            var user = _context.Users
                .FirstOrDefault(usr => usr.UserName == userName);
            if (user != null)
            {
                var passwordCorrect = user.Password.SequenceEqual(_passwordHasher.GetHash(password, userName));
                if (!passwordCorrect)
                {
                    return Task.FromResult((ClaimsIdentity)null);
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "user")
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return Task.Run(() => claimsIdentity);
            }
            return Task.FromResult((ClaimsIdentity)null);
        }
    }
}
