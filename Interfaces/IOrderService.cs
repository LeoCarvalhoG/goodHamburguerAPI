using GoodHamburgerAPI.Models;

namespace GoodHamburgerAPI.Services
{
    public interface IOrderService
    {
        Task<decimal> CalculateTotalPrice(int? sandwichId, int? friesId, int? softDrinkId);
        Task<ServiceResult<bool>> ValidateOrderItems(Order order);
        Task<ServiceResult<Order>> AddOrderAsync(Order order);
        Task<ServiceResult<bool>> UpdateOrderAsync(int id, Order updatedOrder);
        Task<ServiceResult<IEnumerable<Order>>> GetOrdersAsync();
        Task<ServiceResult<Order>> GetOrderByIdAsync(int id);
        Task<ServiceResult<bool>> DeleteOrderAsync(int id);
    }
}
