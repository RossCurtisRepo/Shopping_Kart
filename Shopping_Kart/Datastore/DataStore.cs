using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Datastore
{
    public class DataStore : DbContext, IDataStore
    {
        internal readonly DataStoreContext context;
        internal DbSet<Product> products;
        public DataStore(DataStoreContext context)
        {
            this.context = context;
            products = context.Set<Product>();
        }

        public override int SaveChanges()
        {
            return context.SaveChanges();
        }
        List<Product> IDataStore.GetAllProducts()
        {
            return products.ToList();
        }
        bool IDataStore.AddProduct(Product product)
        {
            var exists = products.Contains(product);
            if (!exists)
            {
                products.Add(product);
                SaveChanges();
                return true;
            }
            return false;
        }

        Product IDataStore.GetProduct(string sku)
        {
            var di = products.Find(sku.ToLower());
            return di;
        }

        bool IDataStore.UpdateProduct(Product product)
        {
            var exists = products.Contains(product);
            if (exists)
            {
                products.Update(product);
                SaveChanges();
                return true;
            }
            return false;
        }

        bool IDataStore.DeleteProduct(string sku)
        {
            var i = products.Find(sku.ToLower());
            var rc = i != null;
            if (rc)
            {
                products.Remove(i);
                SaveChanges();
            }
            return rc;
        }

        IEnumerable<Product> IDataStore.FindProducts(Expression<Func<Product, bool>> predicate)
        {
            return products.Where(predicate);
        }

        bool IDataStore.ProductExists(string sku)
        {
            var rc = false;
            var i = products.Find(sku.ToLower());
            if (i != null) rc = true;

            return rc;
        }
    }

}
