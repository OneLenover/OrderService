using OrderService.API.DTOs;
using OrderService.API.UseCases.CreateOrder;
using OrderService.DataAccess.Postgres.Entities;
using Riok.Mapperly.Abstractions;

namespace OrderService.API.Mappings
{
    [Mapper]
    public partial class OrderMapper
    {
        public partial OrderDto ToOrderDto(Order order);

        [MapperIgnoreTarget(nameof(Order.Id))]
        [MapperIgnoreTarget(nameof(Order.CreatedAt))]
        public partial Order ToOrder(CreateOrderCommand command);

        [MapperIgnoreTarget(nameof(OrderCreatedEvent.Type))]
        [MapperIgnoreSource(nameof(Order.CreatedAt))]
        public partial OrderCreatedEvent ToOrderCreatedEvent(Order order);
    }
}
