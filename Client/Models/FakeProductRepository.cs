using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class FakeProductRepository /*: IProductRepository*/
    {
        public IQueryable<Product> Products => new List<Product> {
            new Product { Name = "Cage", Price = 200 },
            new Product { Name = "Dog food", Price = 50 },
            new Product { Name = "Cat food", Price = 50 },
            new Product { Name = "Parrot food", Price = 45 }
        }.AsQueryable<Product>();
    }
}
