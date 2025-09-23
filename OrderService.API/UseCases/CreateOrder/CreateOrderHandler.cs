using MediatR;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Entities;

namespace OrderService.API.UseCases.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private readonly IAppDbContext _context;

        public CreateOrderHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                ProductId = request.ProductId,
                Amount = request.Amount,
                EmailClient = request.EmailClient,
                Price = request.Price,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
