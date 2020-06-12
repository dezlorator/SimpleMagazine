using PetStore.Models;
using PetStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
