namespace PhoneVault.Models
{
    public class AdminLogDTO
    {
        public int AdminId { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
