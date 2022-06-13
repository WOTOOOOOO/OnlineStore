using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.ViewModels;

namespace someOnlineStore.Data.ViewComponents
{
    public class ProfileEdits : ViewComponent
    {
        public IViewComponentResult Invoke(ProfileGeneralEditsVM userVM)
        {
            return View(userVM);
        }
    }
}
