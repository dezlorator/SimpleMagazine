using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Cart
    {
        #region fields

        private List<CartLine> _lineCollection = new List<CartLine>(); 

        #endregion

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection
                .Where(p => p.Product.ID == product.ID)
                .FirstOrDefault();

            if (line == null)
            {
                _lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void ReduceQuantity(Product product)
        {
            foreach (var l in _lineCollection)
            {
                if (l.Product.ID == product.ID)
                {
                    l.Quantity--;
                    break;
                }
            }
        }

        public virtual void RemoveLine(Product product) =>
            _lineCollection.RemoveAll(l => l.Product.ID == product.ID);

        public virtual decimal ComputeTotalValue() =>
            _lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => _lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => _lineCollection;
    }
}
