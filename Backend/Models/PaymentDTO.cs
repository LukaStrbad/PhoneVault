namespace PhoneVault.Models
{
    public class PaymentDTO
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
    }
}
