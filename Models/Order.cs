using PhoneVault.Models;

namespace PhoneVault.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        //public User User { get; set; }
        //public Payment Payment { get; set; }
        //public Shipping Shipping { get; set; }

    }
}
