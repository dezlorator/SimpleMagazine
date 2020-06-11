using _3Lab.Models.ViewModels;
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
            var userRole = new UserRole();
            _context.UserRole.Add(userRole);
            await _context.SaveChangesAsync();
            applicationUser.Role = userRole;
            _context.Users.Add(applicationUser);
            try
            {
                await _context.SaveChangesAsync();
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

        public async Task UpdateUserRole(ChangeUserPermissionViewModel userRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userRole.UserId);
            user.Role.CanAddComments = userRole.CanAddComments;
            user.Role.CanAddProducts = userRole.CanAddProducts;
            user.Role.CanDeleteProducts = userRole.CanDeleteProducts;
            user.Role.CanEditProducts = userRole.CanEditProducts;
            user.Role.CanModerateComments = userRole.CanModerateComments;
            user.Role.CanPurchaseToStock = userRole.CanPurchaseToStock;
            user.Role.CanSetRoles = userRole.CanSetRoles;
            user.Role.CanViewStatistics = userRole.CanViewStatistics;
            user.Role.CanViewUsersList = userRole.CanViewUsersList;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
