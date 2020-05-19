using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models.ViewModels
{
    public class CategoriesChartViewModel
    {
        public string Category { get; set; }
        public List<SimpleChartViewModel> Charts { get; set; }
        public CategoriesChartViewModel()
        {
            Charts = new List<SimpleChartViewModel>();
        }
    }
}
