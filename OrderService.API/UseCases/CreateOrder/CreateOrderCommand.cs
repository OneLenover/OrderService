using MediatR;

namespace OrderService.API.UseCases.CreateOrder
{
    public record CreateOrderCommand(long ProductId, int Amount, string EmailClient, decimal Price, string PhoneNumber) : IRequest<long>;

}
