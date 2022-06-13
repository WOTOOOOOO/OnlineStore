using Microsoft.AspNetCore.Mvc;

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
