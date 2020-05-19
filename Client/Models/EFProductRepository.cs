using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        #region fields

        private ApplicationDbContext _context; 

        #endregion

        public IQueryable<Product> Products => _context.Products;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveProduct(Product product)
        {
            if (product.ID == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product dbEntry = _context.Products
                    .FirstOrDefault(p => p.ID == product.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageId = product.ImageId;
                }
            }
            _context.SaveChanges();
        }

        public Product DeleteProduct(int productID)
        {
            Product dbEntry = _context.Products
                .FirstOrDefault(p => p.ID == productID);
            if (dbEntry != null)
            {
                _context.Products.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
