using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public interface IStockRepository
    {
        IQueryable<Stock> StockItems { get; }
        void SaveStockItem(Stock stock);
        Stock DeleteStockItem(int productID);
        void ReduceQuantity(int productID, int quantity = 1);
    }
}
