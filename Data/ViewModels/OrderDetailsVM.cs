using someOnlineStore.Models;

namespace someOnlineStore.Data.ViewModels
{
    public class OrderDetailsVM
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string adress { get; set; }

        public double total { get; set; }

        public List<OrderItem> items { get; set; }

    }
}
