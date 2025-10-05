using MediatR;

namespace OrderService.API.UseCases.DeleteOrder
{
    // Команды удаления заказа
    public record DeleteOrderCommand(long OrderId) : IRequest<bool>;
}
