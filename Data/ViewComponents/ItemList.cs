using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Data.Enums;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Models;

namespace someOnlineStore.Data.ViewComponents
{
    public class ItemList : ViewComponent
    {
        private readonly IProductsService _productService;

        public ItemList(IProductsService productsService)
        {
            _productService = productsService;
        }

        public IViewComponentResult Invoke(IEnumerable<Products> products , Category categories, String searchString)
        {
            if (products == null && categories == Category.None && searchString == null)
            {
                var productList = _productService.GetAllAsync().Result;
                return View(productList);
            }
            return View(products);
        }
    }
}
