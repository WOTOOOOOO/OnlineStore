using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.Cart;
using someOnlineStore.Data.EmailData;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.Static;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;
using System.Net.Mail;

namespace someOnlineStore.Areas.Account.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mailService;
        private readonly ShoppingCart _cart;

        public AccountController(UserManager<User> userManager, ShoppingCart cart, 
            SignInManager<User> signInManager, IMailService mailService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _cart = cart;
        }

        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);

                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                TempData["Error"] = "wrong credentials";
                return View(loginVM);
            }

            TempData["Error"] = "wrong credentials";
            return View(loginVM);
        }


        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if (user != null)
            {
                TempData["Error"] = "Email adress already in use.";
                return View(registerVM);
            }

            user = await _userManager.FindByNameAsync(registerVM.Username);

            if (user != null)
            {
                TempData["Error"] = "Username already in use.";
                return View(registerVM);
            }

            var newUser = new User()
            {
                Email = registerVM.Email,
                PhoneNumber = registerVM.phoneNumber,
                UserName = registerVM.Username,
                Adress = registerVM.Adress
            };

            foreach (IPasswordValidator<User> validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, newUser, registerVM.Password);
                if (!validationResult.Succeeded)
                {
                    TempData["Error"] = validationResult.Errors.First().Description;
                    return View(registerVM);
                }
            }

            try
            {
                MailAddress m = new MailAddress(newUser.Email);
            }
            catch (FormatException)
            {
                TempData["Error"] = "Invalid email adress";
                return View(registerVM);
            }

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = newUser.Id, token = token }, Request.Scheme);

                var message = new Message(new List<string>() { newUser.Email }, "Email Confirmation",
                    Constants.generateConfirmationEmail(confirmationLink), null);

                await _mailService.SendEmailAsync(message);

                return RedirectToAction("Login", "Account");
            }

            TempData["Error"] = "Registration Unsuccessful";
            return View(registerVM);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            TempData["ErrorTitle"] = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _cart.ClearCart();
            HttpContext.Session.Remove("PCLink");
            HttpContext.Session.Remove("ECLink");
            HttpContext.Session.Remove("CartId");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
