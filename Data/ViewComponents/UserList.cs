using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Models;

namespace someOnlineStore.Data.ViewComponents
{
    public class UserList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<User> userList)
        {
            return View(userList);
        }
    }
}
