using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Filters.FilterParameters
{
    public class FilterParametersProducts
    {
        public string Name { get; set; } = String.Empty;
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int Category { get; set; }
        public string Categories { get; set; }
        public List<int> CategoriesList { get; set; } 
    }
}
