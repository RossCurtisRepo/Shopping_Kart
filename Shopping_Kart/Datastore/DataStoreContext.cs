using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Datastore
{
    public class DataStoreContext : DbContext
    {
        public DbSet<Product>  Products { get; set; }
        public DataStoreContext(DbContextOptions<DataStoreContext> options): base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
