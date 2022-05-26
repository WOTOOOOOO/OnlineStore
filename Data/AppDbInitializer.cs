using Microsoft.AspNetCore.Identity;
using someOnlineStore.Data.Enums;
using someOnlineStore.Data.Static;
using someOnlineStore.Models;

namespace someOnlineStore.Data
{
    public class AppDbInitializer
    {
        public static void seed(ApplicationDbContext _context)
        {
            _context.Database.EnsureCreated();

            if (!_context.products.Any())
            {
                var products = new List<Products>()
                {
                    new Products
                    {
                        ProductName = "woto",
                        ProductDescription = "his amazing",
                        price = 69.429,
                        image = "/images/default.png",
                        Categories = Category.Accessories | Category.Beauty
                    }
                };
                _context.products.AddRange(products);
                _context.SaveChanges();
            }
        }


        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {

            //Roles
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                string adminUserEmail = "woto@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        Adress = "mars",
                        PhoneNumber = "551696969",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "1234");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            
        }

    }
}
