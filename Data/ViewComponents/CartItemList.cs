using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.Cart;
using someOnlineStore.Data.ViewModels;

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

            var cartvm = new CartVM()
            {
                Total = _cart.GetCartTotal(),
                items = items
            };
            return View(cartvm);
        }

    }
}
