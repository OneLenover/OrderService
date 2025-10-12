using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;

namespace OrderService.API.UseCases.DeleteOrder
{
    // Команды удаления заказа
    public record DeleteOrderCommand(long OrderId) : IRequest<bool>;

    // Обработик удаления заказа
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IAppDbContext _db;

        public DeleteOrderHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            // Удаление заказа
            if (order == null) return false;

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
