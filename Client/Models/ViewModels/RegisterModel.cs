using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models.ViewModels
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [UIHint("password")]
        public string PasswordConfirm { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
