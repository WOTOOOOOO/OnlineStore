using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.ViewModels;

namespace someOnlineStore.Data.ViewComponents
{
    public class LoginForm : ViewComponent
    {
        public IViewComponentResult Invoke(LoginVM loginVM)
        {
            return View(loginVM);
        }
    }
}
