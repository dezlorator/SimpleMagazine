using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models.ViewModels
{
    public class CartIndexViewModel
    {
        #region properties

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } 
        public int EditedLineId { get; set; }

        #endregion
    }
}
