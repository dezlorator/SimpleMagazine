using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _3Lab.Models;
using _3Lab.Models.ViewModels;
using LearningEngine.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace _3Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IEFUserRepository _userRepository;
        private const int PageSize = 2; 

        public UserController(ApplicationDbContext cotenxt,
                              IEFUserRepository userRepository)
        {
            _context = cotenxt;
            _userRepository = userRepository;
        }

        [HttpPost("GetModel")]
        public async Task<ActionResult> GetModel(int userToEdit)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userToEdit);

            if (user == null)
            {
                return NotFound();
            }

            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == user.RoleId);

            var changeUserPermissionViewModel = new ChangeUserPermissionViewModel
            {
                UserId = user.Id,
                CanPurchaseToStock = role.CanPurchaseToStock,
                CanDeleteProducts = role.CanDeleteProducts,
                CanSetRoles = role.CanSetRoles,
                CanViewStatistics = role.CanViewStatistics,
                CanAddComments = role.CanAddComments,
                CanAddProducts = role.CanAddProducts,
                CanEditProducts = role.CanEditProducts,
                CanModerateComments = role.CanModerateComments,
                CanViewUsersList = role.CanViewUsersList
            };

            return Ok(changeUserPermissionViewModel);
        }

        [HttpPut("ChangePermission")]
        public async Task<ActionResult> ChangePermission([FromForm] ChangeUserPermissionViewModel filter)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role?.CanSetRoles == true)
            {
                try
                {
                    await _userRepository.UpdateUserRole(filter);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }

                return Ok();
            }

            return Forbid();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromForm] int productPage = 1)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanViewUsersList == false)
            {
                //return Forbid();
            }

            var users = _context.Users;

            foreach (var user in users)
            {
                user.Role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == user.RoleId);
                user.Role.User = null;
            }

            var paging = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = users.Count()
            };

            return Ok(new 
            { 
                applicationUsers = users
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize),
                pagingInfo = paging
            }
            );
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([FromForm] int userId)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanDeleteUsers == false)
            {
                return Forbid();
            }

            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
