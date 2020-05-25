using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetStore.Filters.FilterParameters;
using PetStore.Models;

namespace PetStore.Filters
{
    public class FilterConditionsProducts : IFilterConditionsProducts
    {
        public IQueryable<Product> GetProducts(IQueryable<Product> products, FilterParametersProducts filter)
        {
            var categories = new List<int>();
            var categoriesStrings = filter.Categories.Split(';');

            foreach (var category in categoriesStrings)
            {
                categories.Add(Convert.ToInt32(category));
            }

            products = products.Where(p => filter.Categories == null || categories.Contains(p.Category.ID));

            if (!String.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(c => c.Name.ToUpper().Contains(filter.Name.ToUpper()));
            }

            if (filter.MinPrice > 0)
            {
                products = products.Where(c => c.Price >= filter.MinPrice);
            }

            if (filter.MaxPrice > 0)
            {
                products = products.Where(c => c.Price <= filter.MaxPrice);
            }

            return products;
        }

        public IQueryable<Stock> GetStockProducts(IQueryable<Stock> stockProducts, FilterParametersProducts filter)
        {
            var categories = new List<int>();
            var categoriesStrings = filter.Categories.Split(';');

            foreach (var category in categoriesStrings)
            {
                categories.Add(Convert.ToInt32(category));
            }

            stockProducts = stockProducts.Where(p => filter.Categories == null || categories.Contains(p.Product.Category.ID));

            if (!String.IsNullOrEmpty(filter.Name))
            {
                stockProducts = stockProducts.Where(c => c.Product.Name.ToUpper().Contains(filter.Name.ToUpper()));
            }

            if (filter.MinPrice > 0)
            {
                stockProducts = stockProducts.Where(c => c.Product.Price >= filter.MinPrice);
            }

            if (filter.MaxPrice > 0)
            {
                stockProducts = stockProducts.Where(c => c.Product.Price <= filter.MaxPrice);
            }

            return stockProducts;
        }
    }
}
