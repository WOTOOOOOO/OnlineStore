using Microsoft.AspNetCore.Mvc;

namespace someOnlineStore.Data.ViewComponents
{
    public class CartSummary : ViewComponent
    {

        public CartSummary() { }

        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
