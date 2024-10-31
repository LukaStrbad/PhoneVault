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
        public decimal SelltPrice { get; set; }
        public int QuantityInStock  { get; set; }
        public int CategoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Category Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}

