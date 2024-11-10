using System.Text.Json.Serialization;

namespace PhoneVault.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        // Relationships
        public int ShoppingCartId { get; set; }
        [JsonIgnore] public ShoppingCart? ShoppingCart { get; set; }
        [JsonIgnore] public Product? Product { get; set; }
        public int ProductId { get; set; }
    }
}