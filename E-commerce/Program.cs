using E_commerce.Context;
using E_commerce.Services;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Stripe;


var builder = WebApplication.CreateBuilder(args);

//Stripe Settings Configuration
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

//CONNECTION - DATACONTEXT
var conn = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(conn));

//AUTH
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });


// POLICIES
builder.Services.AddAuthorization(options =>
{

    // Admin Policy
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "admin"); // [Authorize(Policy = "AdminPolicy")]
    });

    //Farmacista Policy
    options.AddPolicy("UserPolicy", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "user"); // [Authorize(Policy = "FarmacPolicy")]
    });

});


//SERVICES
builder.Services
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IProductService, E_commerce.Services.ProductService>()
    .AddScoped<ICategoriesService, CategoriesService>()
    .AddScoped<ICartService, CartService>()
    .AddScoped<IAddressService, AddressService>()
    .AddScoped<IOrderService,OrderService>()
    .AddScoped<IStripePaymentService,StripePaymentService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
