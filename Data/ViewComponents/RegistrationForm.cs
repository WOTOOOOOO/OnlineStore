using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.ViewModels;

namespace someOnlineStore.Data.ViewComponents
{
    public class RegistrationForm: ViewComponent
    {
        public IViewComponentResult Invoke(RegisterVM registerVM)
        {
            return View(registerVM);
        }
    }
}
