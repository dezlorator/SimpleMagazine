using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models.ViewModels
{
    public class ProductWithCategoryViewModel
    {
        public int ID { get; set; }
        public ProductExtended Product { get; set; }
        public IQueryable<CategoryNode> Categories { get; set; }
    }
}
