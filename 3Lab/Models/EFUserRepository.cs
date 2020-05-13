using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3Lab.Models
{
    public class EFUserRepository : IEFUserRepository
    {
        private ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(ApplicationUser applicationUser)
        {
            await _context.Users.AddAsync(applicationUser);
            try
            {
                var result = await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                var a = e;
            }
        }
        public async Task EmailConfirmed(ApplicationUser applicationUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == applicationUser.UserName);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        }

        public async Task<ApplicationUser> FindByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task DeleteUser(ApplicationUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
