namespace PhoneVault.Models
{
    public class ReviewDTO
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
