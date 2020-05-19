using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class CartLine
    {
        #region properties

        public int ID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; } 

        #endregion
    }
}
