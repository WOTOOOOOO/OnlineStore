using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.ViewModels;

namespace someOnlineStore.Data.ViewComponents
{
    public class PasswordChangeForm: ViewComponent
    {
        public IViewComponentResult Invoke(PasswordChangeFormVM passwordChangeFormVM)
        {
            return View(passwordChangeFormVM);
        }
    }
}
