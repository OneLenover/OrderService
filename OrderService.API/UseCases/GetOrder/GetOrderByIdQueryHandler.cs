using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.DTOs;
using OrderService.DataAccess.Postgres;

namespace OrderService.API.UseCases.GetOrder
{
    // Обработчик команды получения заказа по Id
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IAppDbContext _db;

        public GetOrderByIdQueryHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null) return null;

            return new OrderDto(
                order.Id,
                order.ProductId,
                order.Amount,
                order.EmailClient,
                order.Price,
                order.PhoneNumber,
                order.CreatedAt
            );
        }
    }
}
