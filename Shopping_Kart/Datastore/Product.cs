using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime;

namespace Datastore
{
    [Table("Products")]
    [Index(nameof(Sku), IsUnique = true)]
    public class Product
    {
        private Product()
        {
        }
        public Product(string sku, decimal price, string description, bool hasOffer, int offerQty, decimal offerPrice)
        {
            if (string.IsNullOrEmpty(sku)) throw new ArgumentNullException("sku cannot be null");
            Sku = sku;
            Price = price;
            Description = description;
            HasOffer = hasOffer;
            OfferQty = offerQty;
            OfferPrice = offerPrice;
        }

        private string sku;

        [Key]
        public string Sku { 
            get { return sku; }
            set {sku = !string.IsNullOrEmpty(value) ? value.ToLower() : value; } }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool HasOffer { get; set; }
        public int OfferQty { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OfferPrice { get; set; }
    }
}