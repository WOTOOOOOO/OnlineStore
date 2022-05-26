using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Models;

namespace someOnlineStore.Data.Services.ServicesImpl
{
    public class ProductsService : EntityBaseService<Products>,IProductsService
    {
        public ProductsService(ApplicationDbContext _context) : base(_context) 
        {
        }
    }
}
