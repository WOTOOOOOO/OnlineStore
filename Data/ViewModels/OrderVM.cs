using someOnlineStore.Models;
using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class OrderVM
    {
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name required")]
        public string firstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name required")]
        public string lastName { get; set; }

        [Display(Name = "Adress")]
        [Required(ErrorMessage = "Adress required")]
        public string adress { get; set; }

    }
}
