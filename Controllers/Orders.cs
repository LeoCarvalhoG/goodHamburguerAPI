using GoodHamburgerAPI.Models;
using GoodHamburgerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburgerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var validationResult = await _orderService.ValidateOrderItems(order);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            order.TotalPrice = await _orderService.CalculateTotalPrice(order.SandwichId, order.FriesId, order.SoftDrinkId);

            var addResult = await _orderService.AddOrderAsync(order);
            if (!addResult.Success)
            {
                return BadRequest(addResult.ErrorMessage);
            }

            return Ok(new { order.TotalPrice });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            var validationResult = await _orderService.ValidateOrderItems(updatedOrder);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            updatedOrder.TotalPrice = await _orderService.CalculateTotalPrice(updatedOrder.SandwichId, updatedOrder.FriesId, updatedOrder.SoftDrinkId);

            var result = await _orderService.UpdateOrderAsync(id, updatedOrder);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(updatedOrder);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _orderService.GetOrdersAsync();
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }
    }
}
