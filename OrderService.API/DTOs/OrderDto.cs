namespace OrderService.API.DTOs
{
    public record OrderDto (long Id, long ProductId, int Amount, string EmailClient, decimal Price, string PhoneNumber, DateTime CreatedAt);
    
    public record CreateOrderDto(long ProductId, int Amount, string EmailClient, decimal Price, string PhoneNumber);
}
