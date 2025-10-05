namespace OrderService.API.DTOs
{
    // Объекты для передачи данных
    public record OrderDto (long Id, long ProductId, int Amount, string EmailClient, decimal Price, string PhoneNumber, DateTime CreatedAt);
    
    public record CreateOrderDto(long ProductId, int Amount, string EmailClient, decimal Price, string PhoneNumber);
}
