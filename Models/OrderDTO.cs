namespace PhoneVault.Models
{
    public class OrderDTO
    {
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingAddress { get; set; }
    }
}
