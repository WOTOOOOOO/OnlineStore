using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace someOnlineStore.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Adress")]
        public string Adress { get; set; }
    }
}
