using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PetStore.Models
{
    public class EFCategoryRepository : ICategoryRepository
    {
        #region fields

        private ApplicationDbContext _context;

        #endregion

        public IQueryable<CategoryNode> Categories
        {
            get
            {
                return _context.Categories.Include(c => c.Children);
            }
        }

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveCategory(CategoryNode category)
        {
            if (category.ID == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                var dbEntry = _context.Categories
                    .Include(c => c.Children)
                    .FirstOrDefault(p => p.ID == category.ID);

                if (dbEntry != null)
                {
                    dbEntry.Name = category.Name;
                    dbEntry.IsRoot = category.IsRoot;
                    dbEntry.Children = category.Children;
                }
            }
            _context.SaveChanges();
        }

        public CategoryNode DeleteCategory(int categoryID)
        {
            var dbEntry = _context.Categories
                .FirstOrDefault(p => p.ID == categoryID);
            if (dbEntry != null)
            {
                _context.Categories.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
