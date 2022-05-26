using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Email ")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
