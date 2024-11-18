namespace PhoneVault.Models;

public class EmailSettings
{
    public int Id { get; set; }
    public User? User { get; set; }
    public required string UserId { get; set; }
    public EmailType EmailTypes { get; set; } = EmailType.None;
    
    public bool ShouldSendEmail(EmailType emailType) => EmailTypes.HasFlag(emailType);

    [Flags]
    public enum EmailType
    {
        None = 0b0000,
        Order = 0b0001,
        NewProduct = 0b0010
    }
}