using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3Lab.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public bool CanAddComments { get; set; } = true;
        public bool CanModerateComments { get; set; }
        public bool CanEditProducts { get; set; }
        public bool CanPurchaseToStock { get; set; }
        public bool CanDeleteProducts { get; set; }
        public bool CanAddProducts { get; set; }
        public bool CanViewStatistics { get; set; }
        public bool CanViewUsersList { get; set; }
        public bool CanDeleteUsers { get; set; }
        public bool CanSetRoles { get; set; }
        public bool CanManageOrders { get; set; }
        public ApplicationUser User { get; set; }
    }
}
