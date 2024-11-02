using PhoneVault.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhoneVault.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
        public string Status { get; set; } 
        public DateTime EstimatedDelivery { get; set; }

        //public Order Order { get; set; }
    }
}
