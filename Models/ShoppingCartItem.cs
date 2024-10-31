namespace PhoneVault.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Relationships
        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }
}
