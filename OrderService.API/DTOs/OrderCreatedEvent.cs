namespace OrderService.API.DTOs
{
    public class OrderCreatedEvent
    {
        public string Type { get; set; } = "OrderCreated";
        public long OrderId { get; set; }
        public string EmailClient { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
