using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models.ViewModels
{
    public class CategoryCostylViewModel
    {
        public IQueryable<CategoryNode> Categories { get; set; }
        public int ID { get; set; }
    }
}
