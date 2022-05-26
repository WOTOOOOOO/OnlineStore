using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using someOnlineStore.Data.Enums;
using someOnlineStore.Data.Services.ServiceInterfaces;

namespace someOnlineStore.Models
{
    public class Products : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Description")] 
        public string ProductDescription { get; set; }

        [Display(Name = "Price")]
        public double price { get; set; }

        [Display(Name = "Image")]
        public string image { get; set; }

        [Display(Name = "Categories")]
        public Category Categories { get; set; }


        }
}
