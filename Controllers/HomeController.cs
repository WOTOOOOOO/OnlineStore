using Microsoft.AspNetCore.Mvc;
using someOnlineStore.Models;
using System.Diagnostics;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.ViewModels;
using someOnlineStore.Data.Enums;
using someOnlineStore.Data.Cart;

namespace someOnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ShoppingCart _cart;

        public HomeController(IProductsService productsService, IWebHostEnvironment webHostEnvironment,
            ShoppingCart cart)
        {
            _productsService = productsService;
            _webHostEnvironment = webHostEnvironment;
            _cart = cart;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Filter(Category categories, string searchString)
        {
            var products = await _productsService.GetAllAsync();
            if (products != null)
            {
                var filteredProducts = products.Where(p => p.Categories.HasFlag(categories));

                if (searchString != null)
                {
                    filteredProducts = filteredProducts.Where(p =>
                        p.ProductName.ToLower().Contains(searchString.ToLower()) |
                        p.ProductDescription.ToLower().Contains(searchString.ToLower()));
                    return ViewComponent("ItemList",
                        new {products = filteredProducts, categories = categories, searchString = searchString});
                }

                return ViewComponent("ItemList",
                    new {products = filteredProducts, categories = categories, searchString = ""});
            }

            return ViewComponent("ItemList", new {products = products, categories = Category.None, searchString = ""});
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Product = await _productsService.GetByIdAsync(id);
            if (Product == null) return View("NotFound");
            
            return View(Product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,
            [Bind("ProductName,ProductDescription,price,image,Categories")]
            NewProductVM product)
        {
            if (ModelState.IsValid)
            {
                var newProduct = await _productsService.GetByIdAsync(id);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, newProduct.image.TrimStart('/'));
                
                System.IO.File.Delete(path);
                var uniqueFileName = UploadedFile(product);
                newProduct.ProductName = product.ProductName;
                newProduct.ProductDescription = product.ProductDescription;
                newProduct.price = product.price;
                newProduct.image = $"/images/{uniqueFileName}";
                newProduct.Categories = product.Categories;
                await _productsService.UpdateAsync(id, newProduct);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id, Category categories, string searchString)
        {
            var product = await _productsService.GetByIdAsync(Id);
            if (product == null)
            {
                TempData["Error"] = "Product not found";
                return await Filter(categories, searchString);
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, product.image.TrimStart('/'));
            System.IO.File.Delete(path);

            await _productsService.DeleteAsync(Id);
            return await Filter(categories, searchString);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, Category categories, string searchString)
        {
            var product = await _productsService.GetByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Product not found";
                return await Filter(categories, searchString);
            }

            _cart.AddItem(product);
            return await Filter(categories, searchString);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(
            [Bind("ProductName,ProductDescription,price,image,Categories")]
            NewProductVM product)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(product);

                var newProduct = new Products()
                {
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    price = product.price,
                    Categories = product.Categories,
                    image = $"/images/{uniqueFileName}"
                };
                await _productsService.AddAsync(newProduct);
            }

            return RedirectToAction("Index", "Home");
        }

        private string UploadedFile(NewProductVM model)
        {
            string uniqueFileName = null;

            if (model.image != null)
            {
                var ad = _webHostEnvironment.WebRootPath;
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public async Task<IActionResult> Details(int Id)
        {
            var product = await _productsService.GetByIdAsync(Id);
            if (product == null)
            {
                return View("NotFound");
            }

            return View(product);
        }
    }
}