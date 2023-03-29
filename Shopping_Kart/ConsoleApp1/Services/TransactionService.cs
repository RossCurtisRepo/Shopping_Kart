using Datastore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Shopping_Kart.Services
{
    public class TransactionService : ITransactionService
    {
        internal Cart _cart;
        internal IDataStore _ds;
        internal bool enableTotal = false;
        internal bool nextItem = true;
        public TransactionService(IDataStore ds)
        {
            _ds = ds;
            _cart = new Cart();
        }
        public void NewTransaction()
        {
            while (true)
            {
                while (true)
                {
                    string sku = GetSku();
                    if (nextItem == false) break;
                    int qty = GetQty();
                    AddToCart(sku, qty);
                }
                var total = Total_cart();

                //Display total
                foreach (var item in _cart.Items)
                {
                    Console.WriteLine($"{item.Product.Description}  x{item.Quantity}  £{item.linetotal}");
                }
                Console.WriteLine($"Total: {total}");
                Console.WriteLine();

                while (true)
                {
                    Console.WriteLine("New Transaction? Y/N");
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Y)
                    {
                        break;
                    }
                    if (key == ConsoleKey.N)
                    {
                        return;
                    }
                }
            }
        }

        private decimal Total_cart()
        {

            decimal total = 0.0m;
            foreach (var item in _cart.Items)
            {
                if (item.Product.HasOffer)
                {
                    //Ideally we should be pulling an offer as type and have an offer calculator factory to deal with different offer types.
                    var offerQty = decimal.Truncate(item.Quantity / item.Product.OfferQty);
                    var offerTotal = offerQty * item.Product.OfferPrice;
                    var remQty = item.Quantity - (offerQty * item.Product.OfferQty);
                    var remAmt = remQty * item.Product.Price;
                    item.linetotal = offerTotal + remAmt;
                    total += item.linetotal;
                }
                else
                {
                    item.linetotal = item.Quantity * item.Product.Price;
                    total += item.linetotal;
                }
            }
            return total;
        }
        /// <summary>
        /// Get SKU or set the next item flag if second run and enter
        /// </summary>
        private string GetSku()
        {
            //Todo, make a base class for all these generic "Getters" to inherit from, and then override in situations like this.
            string sku = "";
            string? input;
            while (string.IsNullOrEmpty(sku))
            {
                if (enableTotal)
                {
                    Console.WriteLine("Please enter an sku or press enter to total and finish");
                }
                else
                {
                    Console.WriteLine("Please enter an sku");
                }

                input = Console.ReadLine();
                if (enableTotal && input == "")
                {
                    nextItem = false;
                    break;
                }

                if (input != null!)
                {
                    if (_ds.ProductExists(input))
                    {
                        sku = input;
                    }
                    else if (!_ds.ProductExists(input))
                    {
                        Console.WriteLine("SKU invalid, this sku does not exist in the repository, please enter a valid sku");
                    }
                }
            }
            return sku;
        }
        /// <summary>
        /// Get SKU or set the next item flag if second run and enter
        /// </summary>
        private int GetQty()
        {
            //Todo, make a base class for all these generic "Getters" to inherit from.
            int qty = 0;
            string? input;
            while (qty <= 0)
            {
                Console.WriteLine("Please enter a quantity");
                input = Console.ReadLine();
                if (input != null && int.TryParse(input, out _))
                {
                    qty = int.Parse(input);
                }
            }
            return qty;
        }
        /// <summary>
        /// Add to cart
        /// </summary>
        /// <param name="sku">The  SKU of the required product</param>
        /// <param name="qty">The  qty to add</param>
        private void AddToCart(string sku, int qty)
        {
            //Todo - should this be an event?
            if (_cart.Items.Where(i => i.Product.Sku == sku).Any())
            {
                _cart.Items.First(i => i.Product.Sku == sku).Quantity = +qty;
            }
            else
            {
                _cart.Items.Add(new CartItem(_ds.GetProduct(sku), qty));
            }
            //Total should be enabled only when items exist in the cart
            enableTotal = true;
        }
    }
}
