using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Filters.FilterParameters;

namespace PetStore.Filters
{
    public interface IFilterConditionsProducts
    {
        IQueryable<Product> GetProducts(IQueryable<Product> products,
                                    FilterParametersProducts filter);
        IQueryable<Stock> GetStockProducts(IQueryable<Stock> stockProducts,
                                    FilterParametersProducts filter);
    }
}
