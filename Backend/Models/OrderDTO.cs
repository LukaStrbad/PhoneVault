namespace PhoneVault.Models
{
    public class OrderDTO
    {
        public string ShippingAddress { get; set; } = "";
        public List<OrderItemDto> OrderItems { get; set; } = [];
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
