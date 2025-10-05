using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.DTOs;
using OrderService.API.UseCases.CreateOrder;
using OrderService.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OrderService.API.UseCases.GetOrder;
using OrderService.API.UseCases.DeleteOrder;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Базовый путь
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // Создать новый заказ
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Создание заказа: ProductId ={ ProductId}, Email ={ EmailClient}", dto.ProductId, dto.EmailClient);

            var cmd = new CreateOrderCommand(dto.ProductId, dto.Amount, dto.EmailClient, dto.Price, dto.PhoneNumber);
            var id = await _mediator.Send(cmd, cancellationToken);
            _logger.LogInformation("Заказ успешно создан с ID={OrderId}", id);

            return CreatedAtRoute("GetOrderById", new { order_id = id }, new { id });
        }

        // Получить заказ по OrderId
        [HttpGet("{order_id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long order_id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Получение заказа по ID={OrderId}", order_id);

            var query = new GetOrderByIdQuery(order_id);
            var order = await _mediator.Send(query, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning("Заказ с ID={OrderId} не найден", order_id);
                return NotFound();
            }

            return Ok(order);
        }

        // Удалить заказа по OrderId
        [HttpDelete("{order_id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long order_id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Удаление заказа ID={OrderId}", order_id);

            var cmd = new DeleteOrderCommand(order_id);
            var success = await _mediator.Send(cmd, cancellationToken);

            if (!success)
            {
                _logger.LogWarning("Не удалось удалить заказ ID={OrderId}: не найден", order_id);
                return NotFound();
            }

            _logger.LogInformation("Заказ ID={OrderId} успешно удалён", order_id);
            return NoContent();
        }
    }
}
