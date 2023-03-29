using Datastore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Kart.Services
{
    internal class ProductService : IProductService
    {
        internal IDataStore _ds;
        public ProductService(IDataStore ds)
        {
            _ds = ds;

        }
        /// <summary>
        /// Add a new product to database
        /// </summary>
        void IProductService.AddToDB()
        {
            {
                Console.Clear();
                string? sku = null;


                while (string.IsNullOrEmpty(sku))
                {
                    Console.WriteLine("Enter Product SKU");
                    sku = Console.ReadLine();
                    if (sku != null && _ds.ProductExists(sku))
                    {
                        Console.WriteLine("Product already exists in datavase");
                        sku = null;
                    }
                }

                Product product = GetProductInfo(sku);
                
                _ds.AddProduct(product);
                Console.Clear();
                return;
            }

        }
        /// <summary>
        /// Remove a product from Database
        /// </summary>
        public void RemoveFromDB()
        {

            string? sku = null;

            Console.WriteLine("Enter Product SKU");
            while (string.IsNullOrEmpty(sku))
            {
                sku = Console.ReadLine();
                if (sku != null && !_ds.ProductExists(sku))
                {
                    Console.WriteLine("Product does not exist in database");
                    sku = null;
                }
            }
            _ds.DeleteProduct(sku);
            Console.Clear();
            return;
        }
        /// <summary>
        /// Update a product in database
        /// </summary>
        public void UpdateInDB()
        {

            string? sku = null;

            Console.WriteLine("Enter Product SKU");
            while (string.IsNullOrEmpty(sku))
            {
                sku = Console.ReadLine();
                if (sku != null && !_ds.ProductExists(sku))
                {
                    Console.WriteLine("Product does not exist in database");
                    sku = null;
                }
            }

            Product product = GetProductInfo(sku);
            _ds.UpdateProduct(product);
            Console.Clear();
            return;
        }
        /// <summary>
        /// Collects the information for a product
        /// </summary>
        /// <param name="sku">The  SKU of the product</param>
        /// <returns>A <see cref="Product"/></returns>
        public Product GetProductInfo(string sku)
        {
            decimal price = 0;
            string? description = null;
            bool offerApplies = false;
            int offerQuantity = 0;
            decimal offerPrice = 0;
            string? input;
            {
                Console.WriteLine("Enter Price");
                while (price < 0.01m)
                {
                    input = Console.ReadLine();
                    //Price must be in currency format
                    if (input != null && CurrencyExtensions.IsValidCurrency(input) && decimal.Parse(input) > 0)
                    {
                        price = decimal.Parse(input);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid price");
                    }
                }
                Console.WriteLine("Enter Description");
                while (string.IsNullOrEmpty(description))
                {
                    description = Console.ReadLine();
                }


                while (true)
                {
                    Console.WriteLine("Does a special offer apply? Y/N");
                    var key = Console.ReadKey().Key;
                    
                    
                    if (key == ConsoleKey.Y)
                    {
                        offerApplies = true;
                        break;
                    }
                    else if (key == ConsoleKey.N)
                    {
                        offerApplies = false;
                        break;
                    }
                    
                }

                if (offerApplies == true)
                {
                    Console.WriteLine("Enter Quantity");
                    while (offerQuantity < 1)
                    {
                        input = Console.ReadLine();
                        if (int.TryParse(input, out _))
                            offerQuantity = int.Parse(input);
                    }
                    Console.WriteLine("Enter Price for Quantity");
                    while (offerPrice < 0.01m)
                    {
                        input = Console.ReadLine();
                        if (input != null && CurrencyExtensions.IsValidCurrency(input) && decimal.Parse(input) > 0)
                            offerPrice = decimal.Parse(input);
                    }
                }
                return new Product(sku, price, description, offerApplies, offerQuantity, offerPrice);
            }
        }
    }
}
