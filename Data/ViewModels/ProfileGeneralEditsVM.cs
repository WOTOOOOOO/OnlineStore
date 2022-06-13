using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class ProfileGeneralEditsVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Display(Name = "Adress")]
        [Required(ErrorMessage = "Adress is required")]
        public string Adress { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone Number Reaquired")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?([0-9]{3})?([0-9]{3})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
    }
}
