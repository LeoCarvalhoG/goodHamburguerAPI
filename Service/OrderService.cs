using GoodHamburgerAPI.Data;
using GoodHamburgerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateTotalPrice(int? sandwichId, int? friesId, int? softDrinkId)
        {
            // Use FindAsync and convert ValueTask to Task
            var sandwichTask = _context.Sandwiches.FindAsync(sandwichId).AsTask();
            var friesTask = _context.Extras.FindAsync(friesId).AsTask();
            var softDrinkTask = _context.Extras.FindAsync(softDrinkId).AsTask();

            // Await the tasks and get the results
            var sandwich = await sandwichTask;
            var fries = await friesTask;
            var softDrink = await softDrinkTask;

            // Calculate base price
            decimal totalPrice = (sandwich?.Price ?? 0m) +
                                 (fries?.Price ?? 0m) +
                                 (softDrink?.Price ?? 0m);

            // Apply discounts
            decimal discount = 0;

            if (sandwich != null && fries != null && softDrink != null)
            {
                discount = 0.20m; // 20% discount
            }
            else if (sandwich != null && softDrink != null)
            {
                discount = 0.15m; // 15% discount
            }
            else if (sandwich != null && fries != null)
            {
                discount = 0.10m; // 10% discount
            }

            totalPrice -= totalPrice * discount;

            return totalPrice;
        }

        public async Task<ServiceResult<bool>> ValidateOrderItems(Order order)
        {
            // Use FindAsync and convert ValueTask to Task
            var sandwich = await _context.Sandwiches.FindAsync(order.SandwichId);
            var fries = await _context.Extras.FindAsync(order.FriesId);
            var softDrink = await _context.Extras.FindAsync(order.SoftDrinkId);

            // Validate sandwich
            if (sandwich == null)
            {
                return ServiceResult<bool>.ErrorResult("Invalid sandwich.");
            }

            // Validate for duplicate IDs
            var itemIds = new[] { order.SandwichId, order.FriesId, order.SoftDrinkId }
                            .Where(id => id.HasValue).ToArray();
            if (itemIds.Distinct().Count() != itemIds.Length)
            {
                return ServiceResult<bool>.ErrorResult("Order items cannot be the same.");
            }

            // Validate item names with case-insensitive comparison
            if ((order.FriesId.HasValue && !string.Equals(fries?.Name, "Fries", StringComparison.OrdinalIgnoreCase)) ||
                (order.SoftDrinkId.HasValue && !string.Equals(softDrink?.Name, "Soft drink", StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResult<bool>.ErrorResult("Invalid additional items.");
            }

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<Order>> AddOrderAsync(Order order)
        {
            // Validate order items before adding
            var validationResult = await ValidateOrderItems(order);
            if (!validationResult.Success)
            {
                return ServiceResult<Order>.ErrorResult(validationResult.ErrorMessage);
            }

            // Calculate total price before saving
            order.TotalPrice = await CalculateTotalPrice(order.SandwichId, order.FriesId, order.SoftDrinkId);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return ServiceResult<Order>.SuccessResult(order);
        }

        public async Task<ServiceResult<bool>> UpdateOrderAsync(int id, Order updatedOrder)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return ServiceResult<bool>.ErrorResult("Order not found.");

            // Validate updated order items
            var validationResult = await ValidateOrderItems(updatedOrder);
            if (!validationResult.Success)
            {
                return ServiceResult<bool>.ErrorResult(validationResult.ErrorMessage);
            }

            // Calculate new total price
            order.SandwichId = updatedOrder.SandwichId;
            order.FriesId = updatedOrder.FriesId;
            order.SoftDrinkId = updatedOrder.SoftDrinkId;
            order.TotalPrice = await CalculateTotalPrice(updatedOrder.SandwichId, updatedOrder.FriesId, updatedOrder.SoftDrinkId);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<IEnumerable<Order>>> GetOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return ServiceResult<IEnumerable<Order>>.SuccessResult(orders);
        }

        public async Task<ServiceResult<Order>> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return ServiceResult<Order>.ErrorResult("Order not found.");

            return ServiceResult<Order>.SuccessResult(order);
        }

        public async Task<ServiceResult<bool>> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return ServiceResult<bool>.ErrorResult("Order not found.");

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
    }
}
