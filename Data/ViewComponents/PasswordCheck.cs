using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Models;

namespace someOnlineStore.Data.ViewComponents
{
    public class PasswordCheckViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int trigger)
        {
            return View(trigger);
        }
    }
}
