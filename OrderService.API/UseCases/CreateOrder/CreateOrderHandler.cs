using MediatR;
using OrderService.API.Clients;
using OrderService.API.Services;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Entities;
using System.Text.Json;

namespace OrderService.API.UseCases.CreateOrder
{
    // Обработчик команды создания команды
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, long>
    {
        // Внедрение зависимостей
        private readonly IAppDbContext _context;
        private readonly IPaymentClient _paymentClient;
        private readonly KafkaProducer _kafkaProducer;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(IAppDbContext context, IPaymentClient paymentClient, KafkaProducer kafkaProducer, ILogger<CreateOrderHandler> logger)
        {
            _context = context;
            _paymentClient = paymentClient;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
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

            try
            {
                var payment = await _paymentClient.CreatePayment(new CreatePaymentRequest(order.Id, order.Price));

                // Отправка Kafka-события
                var message = JsonSerializer.Serialize(new
                {
                    Type = "OrderCreated",
                    OrderId = order.Id,
                    EmailClient = order.EmailClient,
                    Amount = order.Amount,
                    Price = order.Price
                });

                await _kafkaProducer.ProduceAsync("notifications", message);

                return order.Id;
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                _logger.LogError(ex, "Ошибка при создании платежа для заказа {OrderId}. Откат заказа...", order.Id);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync(cancellationToken);

                throw;
            }            
        }
    }
}
