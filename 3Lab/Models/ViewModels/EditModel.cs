using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models.ViewModels
{
    public class EditModel
    {
        public DateTime Birthday { get; set; }
        public string ReturnUrl { get; set; } = "/";
    }
}
