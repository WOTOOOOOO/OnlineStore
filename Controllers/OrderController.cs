using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.Cart;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;

namespace someOnlineStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ShoppingCart _cart;
        private readonly IProductsService _productsService;
        private readonly UserManager<User> _userManager;
        public OrderController(IOrderService orderService, ShoppingCart cart,
            IProductsService productsService, UserManager<User> userManager)
        {
            _orderService = orderService;
            _cart = cart;
            _productsService = productsService;
            _userManager = userManager;
        }

        public IActionResult ShoppingCart()
        {
            var items = _cart.GetCartItems();
            _cart.CartItems = items;

            var cartvm = new CartVM()
            {
                shoppingCart = _cart,
                Total = _cart.GetCartTotal(),
            };
            return View(cartvm);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int Id)
        {
            var product = await _productsService.GetByIdAsync(Id);
            if (product == null)
            {
                TempData["Error"] = "Product not found";
                return RedirectToAction("ShoppingCart");
            }
            await _cart.RemoveItem(product);
            
            var url = Request.GetTypedHeaders().Referer.ToString();
            return Redirect(url);
        }

        public IActionResult OrderCart()
        {
            var items = _cart.GetCartItems();

            if (items.Count == 0)
            {
                TempData["NoItems"] = "Your cart is empty";
                return RedirectToAction("ShoppingCart");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OrderCart([Bind("firstName,lastName,adress")] OrderVM orderData)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return View("NotFound");
            }

            if (!ModelState.IsValid)
            {
                return View(orderData);
            }

            var cartItems = _cart.GetCartItems();
            var orderId = await _orderService.StoreOrdersAsync(cartItems, userId, orderData);

            return RedirectToAction("Details", new { Id = orderId });
        }

        public async Task<IActionResult> Details(int Id)
        {
            var userId = _userManager.GetUserId(User);
            var order = await _orderService.GetOrderById(Id, userId);
            var orderDetails = new OrderDetailsVM()
            {
                firstName = order.firstName,
                lastName = order.lastName,
                adress = order.adress,
                items = await _orderService.GetOrderItemsById(order.Id)
            };
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order was not found";
                return View("NotFound");
            }

            return View(orderDetails);
        }
    }
}
