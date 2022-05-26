using someOnlineStore.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace someOnlineStore.Data.ViewModels
{
    public class NewProductVM
    {
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name required")]
        public string ProductName { get; set; }

        [Display(Name = "Product Description")]
        [Required(ErrorMessage ="Product description required")]
        public string ProductDescription { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Product price required")]
        public double price { get; set; }

        [Display(Name = "Product Image")]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile image { get; set; }

        [Display(Name = "Categories")]
        public Category Categories { get; set; }
    }
}
