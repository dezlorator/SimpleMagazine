using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PetStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        #region fields

        private ApplicationDbContext _context; 

        #endregion

        public EFOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Orders => _context.Orders
                            .Include(o => o.Lines)
                            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));

            if (order.OrderID == 0)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }

        public Order DeleteOrder(int orderID)
        {
            var dbEntry = _context.Orders
                .Include(o => o.Lines)
                .FirstOrDefault(o => o.OrderID == orderID);
            if (dbEntry != null)
            {
                dbEntry.Lines.Clear();

                _context.Orders.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
