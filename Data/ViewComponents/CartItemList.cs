using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.Cart;

namespace someOnlineStore.Data.ViewComponents
{
    public class CartItemList : ViewComponent
    {
        public readonly ShoppingCart _cart;

        public CartItemList(ShoppingCart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _cart.GetCartItems();
            return View(items);
        }

    }
}
