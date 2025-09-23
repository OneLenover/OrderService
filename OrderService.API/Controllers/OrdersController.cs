using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.DTOs;
using OrderService.API.UseCases.CreateOrder;
using OrderService.DataAccess.Postgres;
using Microsoft.EntityFrameworkCore;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _db;

        public OrdersController(IMediator mediator, IAppDbContext db)
        {
            _mediator = mediator;
            _db = db;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
        {
            var cmd = new CreateOrderCommand(dto.ProductId, dto.Amount, dto.EmailClient, dto.Price, dto.PhoneNumber);
            var id = await _mediator.Send(cmd, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { order_id = id }, new { id });
        }

        [HttpGet("{order_id:long}")]
        public async Task<IActionResult> GetById([FromRoute] long order_id, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == order_id, cancellationToken);

            if (order == null) return NotFound();

            var dto = new OrderDto(order.Id, order.ProductId, order.Amount, order.EmailClient, order.Price, order.PhoneNumber, order.CreatedAt);
            return Ok(dto);
        }

        [HttpDelete("{order_id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long order_id, CancellationToken cancellationToken)
        {
            var order = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == order_id, cancellationToken);
            if (order == null) return NotFound();

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
