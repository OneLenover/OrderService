using AutoMapper;
using MediatR;
using OrderService.API.Clients;
using OrderService.API.DTOs;
using OrderService.API.Services;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Entities;
using System.Text.Json;

namespace OrderService.API.UseCases.CreateOrder
{
    // Команда для создания заказа
    public record CreateOrderCommand(long ProductId, int Amount, string EmailClient, 
        decimal Price, string PhoneNumber) : IRequest<long>;

    // Обработчик команды создания команды
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, long>
    {
        // Внедрение зависимостей
        private readonly IAppDbContext _context;
        private readonly IPaymentClient _paymentClient;
        private readonly KafkaProducer _kafkaProducer;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IAppDbContext context, IPaymentClient paymentClient, 
            KafkaProducer kafkaProducer, ILogger<CreateOrderHandler> logger, IMapper mapper)
        {
            _context = context;
            _paymentClient = paymentClient;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            try
            {
                var payment = await _paymentClient.CreatePayment(new CreatePaymentRequest(order.Id, order.Price));

                var orderEvent = _mapper.Map<OrderCreatedEvent>(order);
                // Отправка Kafka-события
                var message = JsonSerializer.Serialize(orderEvent);

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
