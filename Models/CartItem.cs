using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public Products product  { get; set; }

        public string cartId { get; set; }
    }
}
