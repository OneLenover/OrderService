using MediatR;
using OrderService.API.DTOs;

namespace OrderService.API.UseCases.GetOrder
{
    // Команда получения заказа по Id
    public record GetOrderByIdQuery(long OrderId) : IRequest<OrderDto?>;
}
