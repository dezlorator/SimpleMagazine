using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models.ViewModels
{
    public class ChangeUserPermissionViewModel
    {
        public int UserId { get; set; }
        public bool CanAddComments { get; set; } = true;
        public bool CanModerateComments { get; set; }
        public bool CanEditProducts { get; set; }
        public bool CanPurchaseToStock { get; set; }
        public bool CanDeleteProducts { get; set; }
        public bool CanAddProducts { get; set; }
        public bool CanViewStatistics { get; set; }
        public bool CanViewUsersList { get; set; }
        public bool CanSetRoles { get; set; }
    }
}
