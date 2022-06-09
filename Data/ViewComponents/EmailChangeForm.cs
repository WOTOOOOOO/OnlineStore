using Microsoft.AspNetCore.Mvc;

namespace someOnlineStore.Data.ViewComponents
{
    public class EmailChangeForm : ViewComponent
    {
        public IViewComponentResult Invoke(string Id)
        {
            return View(model: Id);
        }
    }
}
