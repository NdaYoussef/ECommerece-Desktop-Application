using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices;
using ECommerece.Application.IServices.ICartService;
using ECommerece.Application.IServices.ICategoryService;
using ECommerece.Application.IServices.IOrderService;
using ECommerece.Application.IServices.IUserService;
using ECommerece.Application.Mappers;
using ECommerece.Application.Services.CartServices;
using ECommerece.Application.Services.CategoryServices;
using ECommerece.Application.Services.OrderServices;
using ECommerece.Application.Services.ProductServices;
using ECommerece.Application.Services.UserServices;
using ECommerece.Domain.Entities;
using ECommerece.Infrastructure.AppDbContext;
using ECommerece.Infrastructure.Repositories;
using ECommerece.Infrastructure.Seed;
using ECommerece.Presentation.Forms.CategoryForms;
using ECommerece.Presentation.Forms.DashboardForms;
using ECommerece.Presentation.Forms.ProductForms;
using ECommerece.Presentation.Forms.UserForms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerece.Presentation
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            Mapping.RegisterAllMapping();
            ApplicationConfiguration.Initialize();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
                DataSeeder.SeedAdminAsync(db).Wait();
            }

            // ✅ استدعاء واحد بس
            System.Windows.Forms.Application.Run(host.Services.GetRequiredService<LoginForm>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // ===== Presentation =====
                    services.AddTransient<LoginForm>();
                    services.AddTransient<RegisterForm>();
                    services.AddTransient<DashboardForm>();
                    services.AddTransient<CategoryForm>();
                    services.AddTransient<CustomerDashboardForm>();
                    services.AddTransient<AdminProductsForm>();
                    services.AddTransient<AddProductForm>();
                    services.AddTransient<CustomerProductsForm>();
                    //services.AddTransient<CartForm>();
                    //services.AddTransient<AdminOrderManagementForm>();
                    //services.AddTransient<CustomerOrderManagementForm>();

                    // ===== Application Layer =====
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IAccountService, AccountService>();
                    services.AddScoped<ICategoryRepository, CategoryRepository>();
                    services.AddScoped<ICategoryServices, CategoryService>();
                    services.AddScoped<IProductRepository, ProductRepository>();
                    services.AddScoped<IProductService, ProductService>();
                    services.AddScoped<ICartRepository, CartRepository>();
                    services.AddScoped<ICartServices, CartService>();
                    services.AddScoped<IGenericRepository<Product, int>, ProductRepository>();
                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<IOrderAdminService, OrderAdminService>();
                    services.AddScoped<IOrdercustomerService, OrdercustomerService>();

                    // ===== Infrastructure Layer =====
                    services.AddDbContext<ECommerceDbContext>(options =>
                        options.UseSqlServer(
                            context.Configuration.GetConnectionString("DefaultConnection"),
                            sqlOptions => sqlOptions.EnableRetryOnFailure()
                        ));
                });
        }
    }
}