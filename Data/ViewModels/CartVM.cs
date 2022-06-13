using someOnlineStore.Data.Cart;
using someOnlineStore.Models;

namespace someOnlineStore.Data.ViewModels
{
    public class CartVM
    {
        public double Total { get; set; }

        public IEnumerable<CartItem> items { get; set; }
    }
}
