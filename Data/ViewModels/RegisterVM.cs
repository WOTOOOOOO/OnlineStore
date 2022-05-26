using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Adress")]
        [Required(ErrorMessage = "Adress Required")]
        public string Adress { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Reaquired")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?([0-9]{3})?([0-9]{3})$", ErrorMessage = "Not a valid phone number")]
        public string phoneNumber { get; set; }


    }
}
