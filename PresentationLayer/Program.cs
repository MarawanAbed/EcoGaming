using BusinessLogicLayer.Helper;
using BusinessLogicLayer.Mapper;
using BusinessLogicLayer.Services.Abstraction;
using BusinessLogicLayer.Services.Implementation;
using DataAccessLayer.Database;
using DataAccessLayer.Extend;
using DataAccessLayer.Repo.Abstraction;
using DataAccessLayer.Repo.Implementation;
using DataAccessLayer.Seed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace PresentationLayer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Configure SMTP settings
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

            // Configure Stripe settings
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            // Add email services
            builder.Services.AddSingleton<EmailServices>();

            // Configure Entity Framework and SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure Identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

            // Configure AutoMapper
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // Register repositories
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<ICartRepo, CartRepo>();

            // Register services
            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICartServices, CartServices>();
            builder.Services.AddScoped<ICheckoutServices,CheckoutServices>();
            builder.Services.AddScoped<IOrderServices, OrderServices>();

            // Register controllers
            builder.Services.AddScoped<CartController, CartController>();

            // Configure authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = "/Account/Login";
                option.AccessDeniedPath = "/Account/AccessDenied";
            });

            // Add MVC services
            builder.Services.AddControllersWithViews();

            // Add HTTP context accessor
            builder.Services.AddHttpContextAccessor();

            // Add session services
            builder.Services.AddSession();

            // Add distributed memory cache for session storage
            builder.Services.AddDistributedMemoryCache();

            var app = builder.Build();

            // Configure Stripe API key
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // Seed roles and admin user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IdentityDataSeeder.SeedRolesAndAdminUserAsync(services);
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            // Configure default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}