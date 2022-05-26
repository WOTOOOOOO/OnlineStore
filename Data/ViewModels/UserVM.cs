using someOnlineStore.Models;
using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class UserVM
    {
       
        public string Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Adress")]
        public string Adress { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public List<Order> orders { get; set; }
    }
}
