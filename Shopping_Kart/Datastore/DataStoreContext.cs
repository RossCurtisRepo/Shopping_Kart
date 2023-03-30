using Microsoft.EntityFrameworkCore;

namespace Datastore
{
    public class DataStoreContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DataStoreContext(DbContextOptions<DataStoreContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
