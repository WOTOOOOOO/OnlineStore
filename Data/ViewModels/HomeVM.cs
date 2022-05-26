using someOnlineStore.Data.Enums;
using someOnlineStore.Models;

namespace someOnlineStore.Data.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Products> products { get; set; }

        public Category categories { get; set; }

        public string searchString { get; set; }

    }
}
