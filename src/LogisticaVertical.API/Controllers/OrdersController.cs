using LogisticaVertical.Application.Service;
using LogisticaVertical.Domain.Dtos;
using LogisticaVertical.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticaVertical.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly OrderService _processor;
        public OrdersController(OrderService processor)
        {
            _processor = processor;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)] // 100MB
        public async Task<IActionResult> Upload(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            await _processor.ProcessFileAsync(stream);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> Get(
            [FromQuery] int? orderId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var result = await _processor.GetOrdersAsync(orderId, startDate, endDate);
            return Ok(result);
        }
    }
}
