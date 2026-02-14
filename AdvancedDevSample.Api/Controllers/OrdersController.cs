using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AdvancedDevSample.Api.Controllers
{


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart()
        {
            // Crée un panier vide et renvoie son ID
            var id = await _orderService.CreateCartAsync();
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItemToCart(Guid id, [FromBody] AddItemToOrderDto dto)
        {
            await _orderService.AddItemToCartAsync(id, dto);
            return NoContent();
        }

        [HttpPatch("{id}/validate")]
        public async Task<IActionResult> ValidateOrder(Guid id)
        {
            await _orderService.ValidateOrderAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return NoContent();
        }
    }
}
