using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PetStore.Models
{
    public class EFStockRepository : IStockRepository
    {
        #region fields

        private ApplicationDbContext _context;

        #endregion

        public IQueryable<Stock> StockItems => _context.StockItems.Include(i => i.Product);

        public EFStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveStockItem(Stock stock)
        {
            if (stock.ID == 0)
            {
                _context.StockItems.Add(stock);
            }
            else
            {
                Stock dbEntry = _context.StockItems
                    .FirstOrDefault(s => s.ID == stock.ID);
                if (dbEntry != null)
                {
                    dbEntry.Product = stock.Product;
                    dbEntry.Quantity = stock.Quantity;
                }
            }
            _context.SaveChanges();
        }

        public Stock DeleteStockItem(int productID)
        {
            Stock dbEntry = _context.StockItems
                .FirstOrDefault(s => s.Product.ID == productID);

            if (dbEntry != null)
            {
                _context.StockItems.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }

        public void ReduceQuantity(int productID, int quantity = 1)
        {
            Stock dbEntry = _context.StockItems
                .FirstOrDefault(s => s.Product.ID == productID);

            if (dbEntry != null)
            {
                if (dbEntry.Quantity > 0)
                {
                    dbEntry.Quantity -= quantity;
                }

                _context.SaveChanges();
            }
        }
    }
}
