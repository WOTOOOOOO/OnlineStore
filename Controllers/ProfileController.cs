using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.EmailData;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.Static;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Models;
using System.Net.Mail;
using System.Web;


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


        //General edits rrelated
        [HttpPost]
        public async Task<IActionResult> Edit(string Username, string Adress, string PhoneNumber)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return RedirectToAction("Index", "Home");
            user.UserName = Username;
            user.Adress = Adress;
            user.PhoneNumber = PhoneNumber;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }


        //Passwordpopup related
        public IActionResult CheckPasswordPopup(int trigger)
        {
            return ViewComponent("PasswordCheck", new { trigger = trigger });
        }

        public async Task<bool> CheckPassword(string password)
        {
            var user = await _userManager.GetUserAsync(User);
            if (password == null) return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public IActionResult PasswordIncorrect(int trigger)
        {
            TempData["Error"] = "Incorrect password";
            return ViewComponent("PasswordCheck", new { trigger = trigger });
        }

        [HttpGet]
        public async Task<string> PasswordCorrect(int trigger)
        {
            var user = await _userManager.GetUserAsync(User);

            if (trigger == 1) return $"/Profile/ChangeEmail/{user.Id}";
            else if (trigger == 2)
            {

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var confirmationLink = Url.Action("ChangePassword", "Profile", new { Id = user.Id, token = token }, Request.Scheme);

                HttpContext.Session.SetString("PCLink", confirmationLink);

                await _mailService.SendEmailAsync(new Message(new List<string>() { user.Email },
                    "Password reset confirmation", Constants.generatePasswordResetMail(confirmationLink), null));

                return "/Profile/VerificationEmailSent";
            }

            TempData["Error"] = "Unable to change password";
            return "/Profile/Profile";
        }


        // Email change related
        public IActionResult ChangeEmail(string Id)
        {
            if (Id == null) return View("NotFound");

            return View(model: Id);
        }

        public async Task<bool> CheckEmailValidity(string newEmail)
        {
            if (newEmail == null)
            {
                TempData["Error"] = "Email can't be empty";
                return false;
            }

            try
            {
                MailAddress m = new MailAddress(newEmail);
            }
            catch (FormatException)
            {
                TempData["Error"] = "Invalid email adress";
                return false;
            }

            var emailCheck = await _userManager.FindByEmailAsync(newEmail);

            if (emailCheck != null)
            {
                TempData["Error"] = "This email is already in use";
                return false;
            }

            return true;
        }

        public IActionResult EmailInvalid(string Id)
        {
            return ViewComponent("EmailChangeForm", new { Id = Id });
        }

        public async Task<string> EmailValid(string Id, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(Id);

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            var confirmationLink = Url.Action("ConfirmEmailChange", "Profile", new { Id = user.Id, token = token, email = newEmail }, Request.Scheme);

            HttpContext.Session.SetString("ECLink", confirmationLink);

            await _mailService.SendEmailAsync(new Message(new List<string>() { newEmail }, "Email change confirmation",
                Constants.generateEmailChangeConfirmationMail(confirmationLink), null));

            return "/Profile/VerificationEmailSent";
        }

        public async Task<IActionResult> ConfirmEmailChange(string Id, string token, string email)
        {
            if (Id == null || token == null) return RedirectToAction("Index", "home");


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


        //Verification email handling
        public IActionResult VerificationEmailSent()
        {
            var ECLink = HttpContext.Session.GetString("ECLink");
            var PCLink = HttpContext.Session.GetString("PCLink");

            if (ECLink != null) TempData["ECResend"] = true;
            if (PCLink != null) TempData["PCResend"] = true;

            return View();
        }

        public async Task ResendMessage(int trigger)
        {
            var user = await _userManager.GetUserAsync(User);
            if (trigger == 1)
            {
                var ECLink = HttpContext.Session.GetString("ECLink");
                await _mailService.SendEmailAsync(new Message(new List<string>() { user.Email },
                    "Password reset confirmation", Constants.generatePasswordResetMail(ECLink), null));
            }
            else if (trigger == 2)
            {
                var PCLink = HttpContext.Session.GetString("PCLink");
                await _mailService.SendEmailAsync(new Message(new List<string>() { user.Email }, "Password reset confirmation",
                        Constants.generatePasswordResetMail(PCLink), null));
            }
        }


        // Password change related
        public async Task<IActionResult> ChangePassword(string Id, string token)
        {

            if (Id == null || token == null) return RedirectToAction("Index", "Home");

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                TempData["Error"] = $"The User ID {Id} is invalid";
                return View("NotFound");
            }
            var data = new PasswordChangeVM { Id = Id, token = token };
            return View(data);
        }

        public async Task<bool> CheckPasswordValidity(string Id, string token,
            [Bind("newPassword,confirmPassword")] PasswordVM newPasswordVM)
        {

            token = HttpUtility.UrlDecode(token).Replace(' ', '+');

            if (!ModelState.IsValid) return false;

            var user = await _userManager.FindByIdAsync(Id);

            foreach (IPasswordValidator<User> validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, user, newPasswordVM.newPassword);
                if (!validationResult.Succeeded)
                {
                    TempData["Error"] = validationResult.Errors.First().Description;
                    return false;
                }
            }

            return true;
        }

        public IActionResult PasswordInValid(string Id, string token,
            [Bind("newPassword,confirmPassword")] PasswordVM newPasswordVM)
        {
            return ViewComponent("PasswordChangeForm", new PasswordChangeFormVM
            {
                Id = Id,
                token = token,
                newPassword = newPasswordVM.newPassword,
                confirmPassword = newPasswordVM.confirmPassword
            });
        }

        [HttpPost]
        public async Task<bool> PasswordValid(string Id, string token,
            [Bind("newPassword,confirmPassword")] PasswordVM newPasswordVM)
        {
            token = HttpUtility.UrlDecode(token).Replace(' ', '+');

            var user = await _userManager.FindByIdAsync(Id);

            var result = await _userManager.ResetPasswordAsync(user, token, newPasswordVM.newPassword);

            if (!result.Succeeded)
                TempData["Error"] = "Password change was unsuccesful";

            return result.Succeeded;

        }

        public string PasswordChangeSuccessful() => "/Profile/PasswordChanged";

        public IActionResult PasswordChangeUnsuccessful() => View("Error");

        public IActionResult PasswordChanged() => View();

    }
}
