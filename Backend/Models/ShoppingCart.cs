﻿namespace PhoneVault.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = [];
        public string UserId { get; set; }
    }
}
