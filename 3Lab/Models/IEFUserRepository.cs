using _3Lab.Models.ViewModels;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3Lab.Models
{
    public interface IEFUserRepository
    {
        Task RegisterUser(ApplicationUser applicationUser);
        Task EmailConfirmed(ApplicationUser applicationUser);
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<ApplicationUser> FindByIdAsync(int id);
        Task DeleteUser(ApplicationUser user);
        Task UpdateUserRole(ChangeUserPermissionViewModel userRole);
    }
}
