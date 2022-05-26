using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;

namespace someOnlineStore.Data.Services.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<int> StoreOrdersAsync(List<CartItem> items, string userId, OrderVM orderData);

        Task<List<Order>> GetAllOrders(string userId);

        Task<Order> GetOrderById(int Id, string userId);

        Task<List<OrderItem>> GetOrderItemsById(int Id);
    }
}
