using Microsoft.EntityFrameworkCore;
using someOnlineStore.Data.Enums;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;

namespace someOnlineStore.Data.Services.ServicesImpl
{
    public class OrderService : IOrderService
    {
        public ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllOrders(string userId) => await _context.orders.Where(O => O.UserId == userId).ToListAsync();

        public async Task<Order> GetOrderById(int Id, string userId) => await _context.orders.FirstOrDefaultAsync(O => O.UserId == userId && O.Id == Id);

        public async Task<List<OrderItem>> GetOrderItemsById(int Id) => await _context.orderItems.Where(I => I.OrderId == Id).Include(I => I.product).ToListAsync();
        public async Task<int> StoreOrdersAsync(List<CartItem> items, string userId, OrderVM orderData)
        {
            var order = new Order()
            {
                UserId = userId,
                Total = (from item in items select item.product.price).Sum(),
                firstName = orderData.firstName,
                lastName = orderData.lastName,
                adress = orderData.adress,
                orderStatus = OrderStatus.Processing
            };
            await _context.orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = item.product.Id
                };
                await _context.orderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();

            return order.Id;
        }


    }
}
