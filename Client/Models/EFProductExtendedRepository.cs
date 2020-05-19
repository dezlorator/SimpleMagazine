using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class EFProductExtendedRepository : IProductExtendedRepository
    {
        #region private

        private ApplicationDbContext _context;

        #endregion

        public IQueryable<ProductExtended> ProductsExtended => _context.ProductsExtended.Include(i=>i.Comments).Include(i => i.Product).ThenInclude(i=>i.Category);

        public EFProductExtendedRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductExtended DeleteProductExtended(int productExtendedID)
        {
            ProductExtended dbEntry = _context.ProductsExtended
                .FirstOrDefault(p => p.ID == productExtendedID);
            if (dbEntry != null)
            {
                foreach (var c in dbEntry.Comments)
                {
                    _context.Comments.Remove(c);
                }
                _context.ProductsExtended.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveProductExtended(ProductExtended productExtended)
        {
            if (productExtended.ID == 0)
            {
                _context.ProductsExtended.Add(productExtended);
            }
            else
            {
                ProductExtended dbEntry = _context.ProductsExtended
                    .FirstOrDefault(p => p.ID == productExtended.ID);
                if (dbEntry != null)
                {
                    dbEntry.Comments = productExtended.Comments;
                    dbEntry.Product = productExtended.Product;
                    dbEntry.LongDescription = productExtended.LongDescription;
                    dbEntry.Manufacturer = productExtended.Manufacturer;
                    dbEntry.OriginCountry = productExtended.OriginCountry;
                    dbEntry.Image = productExtended.Image;
                }
            }
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
