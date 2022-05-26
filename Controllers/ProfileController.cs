using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.EmailData;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.Static;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;

namespace someOnlineStore.Controllers
{
    public class ProfileController : Controller
    { 
        private readonly IOrderService _orderService;
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public ProfileController(IOrderService orderService, UserManager<User> userManager, 
            IMailService mailService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _mailService = mailService;
        }


        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return View("NotFound");

            var userVM = new UserVM()
            {
                Id = user.Id,
                Username = user.UserName,
                Adress = user.Adress,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                orders = await _orderService.GetAllOrders(user.Id)
            };
            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string Username, string Adress, string PhoneNumber)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            user.UserName = Username;
            user.Adress = Adress;
            user.PhoneNumber = PhoneNumber;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> checkPassword(int trigger, string password)
        {
            var user = await _userManager.GetUserAsync(User);
            if (password == null)
            {
                TempData["Error"] = "Password can't be empty";
                TempData["Trigger"] = trigger;
                return RedirectToAction("Profile");
            }
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                TempData["Error"] = "Incorrect password";
                TempData["Trigger"] = trigger;
                return RedirectToAction("Profile");
            }
            if (trigger == 1)
            {
                return Redirect($"changeEmail/{user.Id}");
            }
            else if (trigger == 2)
            {

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var confirmationLink = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Profile/changePassword/{user.Id}/{token}";

                var message = new Message(new List<string>() { user.Email }, "Password reset confirmation",
                    Constants.generatePasswordResetMail(confirmationLink), null);

                await _mailService.sendEmail(message);

                return RedirectToAction("Index","Home");
            }

            TempData["Error"] = "Unable to change password";
            return RedirectToAction("Profile"); 
        }

        public IActionResult changeEmail(string Id)
        {
            if(Id == null)
            {
                return View("NotFound");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> changeEmail(string Id,string newEmail)
        {
            if (newEmail == null)
            {
                TempData["Error"] = "Email can't be empty";
                return View();
            }

            var emailCheck = await _userManager.FindByEmailAsync(newEmail);

            if (emailCheck != null)
            {
                TempData["Error"] = "This email is already in use";
                return View();
            }

            var user = await _userManager.FindByIdAsync(Id);

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            var confirmationLink = Url.Action("ConfirmEmailChange", "Profile",
                    new { Id = user.Id, token = token, email = newEmail }, Request.Scheme);

            var message = new Message(new List<string>() { newEmail }, "Email change confirmation",
                Constants.generateEmailChangeConfirmationMail(confirmationLink), null);

            await _mailService.sendEmail(message);

            return RedirectToAction("Profile");
        }


        public async Task<IActionResult> changePassword(string Id, string token) 
        {
            if (Id == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                TempData["ErrorMessage"] = $"The User ID {Id} is invalid";
                return View("NotFound");
            }

            return View();
        }

        [Route("changePassword/{Id}/{token}")]
        [HttpPost]
        public async Task<IActionResult> changePassword(string Id, string token,
            [Bind("newPassword,confirmPassword")] PasswordVM newPasswordVM)
        {
            

            if (!ModelState.IsValid)
            {
                return View(newPasswordVM);
            }

            var user = await _userManager.FindByIdAsync(Id);

            var result = await _userManager.ResetPasswordAsync(user, token, newPasswordVM.newPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("PasswordChanged");
            }

            TempData["Error"] = "Password reset was unsuccesful";
            return View("Error");
        }

        public IActionResult PasswordChanged() => View();

        public async Task<IActionResult> ConfirmEmailChange(string Id, string token, string email)
        {
            if (Id == null || token == null)
            {
                return RedirectToAction("Index", "home");
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"The User ID {Id} is invalid";
                return View("NotFound");
            }

            user.Email = email;
            user.NormalizedEmail = email.ToUpper();
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Profile");
        }

    }
}
