namespace OrderService.API.DTOs
{
    public class OrderCreatedEvent
    {
        public string Type { get; set; } = "OrderCreated";
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string EmailClient { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
