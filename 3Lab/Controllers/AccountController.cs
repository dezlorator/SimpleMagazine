using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStore.Models.ViewModels;
using PetStore.Models;
using Microsoft.AspNetCore.Http;
using _3Lab.Auth;
using Microsoft.AspNetCore.Identity;
using _3Lab.Models;
using _3Lab.Services;
using LearningEngine.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace PetStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IPasswordHasher _passwordHasher;
        private IEFUserRepository _userRepository;
        private IJwtTokenCryptographer _jwtTokenCryptographer;
        private IGetClaims _getClaims;
        private ApplicationDbContext _context;

        public AccountController(IPasswordHasher passwordHasher,
                IEFUserRepository userRepository,
                IJwtTokenCryptographer JwtTokenCryptographer,
                IGetClaims getClaims,
                ApplicationDbContext context)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtTokenCryptographer = JwtTokenCryptographer;
            _getClaims = getClaims;
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromForm]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = _passwordHasher.GetHash(model.Password, model.UserName)
                };

                await _userRepository.RegisterUser(user);

                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id },
                    protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                try
                {
                    await emailService.SendEmailAsync(model.Email, "Подтвердите Ваш аккаунт",
                   $"Спасибо за регистрацию в нашем магазине! <br />Для того чтобы приступить к покупкам, подтвердите регистрацию, перейдя по ссылке. <br /><a href='{callbackUrl}'>Подтвердить!</a>");
                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }

            }

            return BadRequest("Wrong email or password");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = _userRepository.EmailConfirmed(user).IsCompletedSuccessfully;
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var claims = await _getClaims.Claims(loginModel.Name, loginModel.Password);

                if (claims == null)
                {
                    return NotFound();
                }

                var encodedJwt = _jwtTokenCryptographer.Encode(claims);

                var user = await _userRepository.FindByNameAsync(loginModel.Name);
                var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == user.RoleId);
                role.User = null;

                var response = new LoginResponse
                {
                    access_token = encodedJwt,
                    username = claims.Name,
                    UserId = user.Id,
                    Role = role
                };

                return Ok(response);
            }
            return BadRequest();
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteConfirmed()
        {
            ApplicationUser user = await _userRepository.FindByIdAsync(this.GetUserId());
            if (user != null)
            {
                await _userRepository.DeleteUser(user);
                Ok();
            }

            return BadRequest();
        }
    }
}