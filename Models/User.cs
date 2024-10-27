using PhoneVault.Models;

namespace PhoneVault.Models
{
    public class User
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }  
        public string UserType { get; set; }
        public DateTime? CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }= DateTime.UtcNow;

        public ICollection<Order> Orders { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}

