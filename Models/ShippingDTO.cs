namespace PhoneVault.Models
{
    public class ShippingDTO
    {
        public int OrderId { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedDelivery { get; set; }
    }
}
