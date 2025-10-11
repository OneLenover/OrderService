using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.DTOs;
using OrderService.DataAccess.Postgres;

namespace OrderService.API.UseCases.GetOrder
{
    // Команда получения заказа по Id
    public record GetOrderByIdQuery(long OrderId) : IRequest<OrderDto?>;

    // Обработчик команды получения заказа по Id
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
    {
        private readonly IAppDbContext _db;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null) return null;

            return _mapper.Map<OrderDto>(order);
        }
    }
}
