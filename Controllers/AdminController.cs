using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.Static;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;

namespace someOnlineStore.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IOrderService _orderService;

        public AdminController(UserManager<User> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        public IActionResult AllUsers()
        {
            return View(_userManager.Users.ToList());
        }

        public async Task<IActionResult> Details(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return View("NotFound");
            }

            var data = new UserVM()
            {
                Id = Id,
                Username = user.UserName,
                Adress = user.Adress,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                orders = await _orderService.GetAllOrders(Id)
            };

            return View(data);
        }

        public async Task<IActionResult> MakeAdmin(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            return ViewComponent("UserList", _userManager.Users.ToList());
        }


        public async Task<IActionResult> DeleteAdmin(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.RemoveFromRoleAsync(user, UserRoles.Admin);
            return ViewComponent("UserList", _userManager.Users.ToList());
        }
    }
}