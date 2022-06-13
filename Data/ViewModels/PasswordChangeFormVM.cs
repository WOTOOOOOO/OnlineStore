using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class PasswordChangeFormVM
    {
        public string Id { get; set; }

        public string token { get; set; }

        [Display(Name = "New password")]
        [Required(ErrorMessage = "Input new password")]
        [DataType(DataType.Password)]

        public string newPassword { get; set; }

        [Display(Name = "Type it again")]
        [Required(ErrorMessage = "Type the password again")]
        [Compare("newPassword")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }
}
