using System.Linq;

namespace PetStore.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
        Order DeleteOrder(int orderId);
    }
}