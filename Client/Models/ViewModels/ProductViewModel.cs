using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public bool IsInStock { get; set; }
    }
}
