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

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Products> products , Category categories, String searchString)
        {
            if (products == null && categories == Category.None && searchString == null)
            {
                var productList =await _productService.GetAllAsync();
                return View(productList);
            }
            return View(products);
        }
    }
}
