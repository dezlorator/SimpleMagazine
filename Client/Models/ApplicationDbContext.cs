using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace PetStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        #region properties

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> StockItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductExtended> ProductsExtended { get; set; }
        public DbSet<CategoryNode> Categories { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}