using Datastore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Kart
{
    public class Cart
    {
        public Cart()
        {
            Items = new List<CartItem>();
        }
        public List<CartItem> Items { get; set; }
    }
    
public class CartItem
    {
        public CartItem(Product prod, int qty)
        {
            Product = prod;
            Quantity = qty;
        }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal linetotal { get; set; }
    }


}
