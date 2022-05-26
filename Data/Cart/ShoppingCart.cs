using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using someOnlineStore.Models;

namespace someOnlineStore.Data.Cart
{
    public class ShoppingCart
    {
        public string CartId { get; set; }

        public ApplicationDbContext _context { get; set; }

        public List<CartItem> CartItems { get; set; }

        public ShoppingCart(ApplicationDbContext context)
        {
            _context=context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider service)
        {
            ISession session = service.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
            var context = service.GetService<ApplicationDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context){ CartId=cartId };
        }

        public void AddItem(Products product)
        {
            var cartItem =_context.cartItems.FirstOrDefault(p => p.product.Id == product.Id && p.cartId == CartId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    product = product,
                    cartId = CartId
                };
                 _context.cartItems.Add(cartItem);
            }
            _context.SaveChanges();
        }

        public async Task RemoveItem(Products product)
        {
            var carItme =await _context.cartItems.FirstOrDefaultAsync(p => p.product.Id == product.Id && p.cartId == CartId);
            if (carItme != null)
            {
                 _context.cartItems.Remove(carItme);
            }
            await _context.SaveChangesAsync();
        }

        public List<CartItem> GetCartItems()
        {
            return CartItems ?? (CartItems = _context.cartItems.Where(a => a.cartId == CartId).Include(a => a.product).ToList());
        }

        public double GetCartTotal() => _context.cartItems.Where(I => I.cartId == CartId).Select(I => I.product.price).Sum();

        public async Task ClearCart()
        {
            var items =await _context.cartItems.Where(I => I.cartId == CartId).ToListAsync();
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task Order()
        {

        }
    }
}
