using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public interface ICategoryRepository
    {
        IQueryable<CategoryNode> Categories { get; }
        void SaveCategory(CategoryNode category);
        CategoryNode DeleteCategory(int categoryID);
        void SaveChanges();
    }
}
