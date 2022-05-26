using someOnlineStore.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace someOnlineStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }   
        [ForeignKey("UserId")]
        public User user { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string adress { get; set; }

        public Double Total { get; set; }

        public OrderStatus orderStatus { get; set; }

        public List<OrderItem> Items { get; set;}
    }
}
