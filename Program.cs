using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using someOnlineStore.Data;
using someOnlineStore.Data.Cart;
using someOnlineStore.Data.EmailData;
using someOnlineStore.Data.Services.ServiceInterfaces;
using someOnlineStore.Data.Services.ServicesImpl;
using someOnlineStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

builder.Services.AddSingleton(builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddIdentity<User,IdentityRole>(o =>
{
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase= false;
    o.Password.RequireNonAlphanumeric= false;
    o.Password.RequireUppercase= false;
    o.Password.RequiredLength = 4;
    o.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});


builder.Services.AddSession();
builder.Services.AddMemoryCache();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "confirm",
        pattern: "{controller=name}/{action=Index}/{id?}/{token?}");
}
);

AppDbInitializer.SeedUsersAndRolesAsync(app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>(),
    app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>()).Wait();
AppDbInitializer.seed(app.Services.CreateScope().ServiceProvider.GetService<ApplicationDbContext>());


app.Run();
// href="@Url.Action("Edit", "Home", new {Area=""})"
