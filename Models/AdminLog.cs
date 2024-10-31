namespace PhoneVault.Models
{
    public class AdminLog
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public User Admin { get; set; }
    }
}
