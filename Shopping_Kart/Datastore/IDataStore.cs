using System.Linq.Expressions;

namespace Datastore
{
    public interface IDataStore : IDisposable
    {

        #region Products
        /// <summary>
        /// Get all existing Products from the datastore
        /// </summary>
        /// <returns>A <see cref="List{Product}"/> list of all stored products></returns>
        public List<Product> GetAllProducts();

        /// <summary>
        /// Add a data item to the product table
        /// </summary>
        /// <param name="Product">The new <see cref="Product"/></param>
        /// <returns>true if successful; otherwise false.</returns>
        bool AddProduct(Product product);

        /// <summary>
        /// Get a data item by unique identifier
        /// </summary>
        /// <param name="sku">The  SKU of the required product</param>
        /// <returns>A <see cref="Product"/> representing the data item sought, or null if the item is not found</returns>
        public Product GetProduct(string sku);

        /// <summary>
        /// Modify existing data using the provided data
        /// </summary>
        /// <param name="Product">The new <see cref="Product"/> to replace the existing data</param>
        /// <returns>A <see cref="Product"/> representing the updated data.</returns>
        bool UpdateProduct(Product product);

        /// <summary>
        /// Remove the provided data item from storage
        /// </summary>
        /// <param name="sku">The  SKU of the required product</param>
        /// <returns>true if successful; otherwise false.</returns>
        public bool DeleteProduct(string sku);
        /// <summary>
        /// Finds all items in the data store that match the provided predicate
        /// </summary>
        /// <param name="predicate">The selection predicate to apply for the search</param>
        /// <returns>A <see cref="Product"/> representing the updated data.</returns>
        public IEnumerable<Product> FindProducts(Expression<Func<Product, bool>> predicate);
        /// <summary>
        /// Checks whether product currently exists
        /// </summary>
        /// <param name="sku">The  SKU of the required product</param>
        /// <returns>true if successful; otherwise false.</returns>
        public bool ProductExists(string sku);

        #endregion
    }
}