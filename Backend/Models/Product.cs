using System.Text.Json.Serialization;
using PhoneVault.Models;

namespace PhoneVault.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public decimal NetPrice { get; set; }
        public decimal SellPrice { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }

        [JsonIgnore] public Category? Category { get; set; } = null;
        [JsonIgnore] public ICollection<Review> Reviews { get; set; } = [];
    }
}