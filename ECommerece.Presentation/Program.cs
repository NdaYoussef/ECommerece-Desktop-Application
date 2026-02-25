using ECommerece.Application.IRepositories;
using ECommerece.Application.IServices;
using ECommerece.Application.IServices.IUserService;
using ECommerece.Application.Mappers;
using ECommerece.Application.Services.ProductServices;
using ECommerece.Application.Services.UserServices;
using ECommerece.Infrastructure.AppDbContext;
using ECommerece.Infrastructure.Repositories;
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
        internal static readonly IHost host = CreateHostBuilder().Build();

        [STAThread]
        static void Main()
        {
            Mapping.RegisterAllMapping();
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.Application.Run(host.Services.GetRequiredService<LoginForm>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(
                    (context, config) =>
                    {
                        config.AddJsonFile(
                            "appsettings.json",
                            optional: false,
                            reloadOnChange: true
                        );
                    }
                )
                .ConfigureServices(
                    (context, services) =>
                    {
                        // Presentation
                        services.AddTransient<LoginForm>();
                        services.AddTransient<RegisterForm>();
                        services.AddTransient<DashboardForm>(); 
                        services.AddTransient<ProductsForm>();
                        services.AddTransient<AddProductForm>();

                        // Application Layer
                        services.AddScoped<IUserRepository, UserRepository>();
                        services.AddScoped<IAccountService, AccountService>();
                        services.AddScoped<IProductRepository, ProductRepository>();
                        services.AddScoped<IProductService, ProductService>();

                        // Infrastructure Layer
                        services.AddDbContext<ECommerceDbContext>(options =>
                            options.UseSqlServer(
                                context.Configuration.GetConnectionString("DefaultConnection"),
                                sqlOptions => sqlOptions.EnableRetryOnFailure()
                            )
                        );
                    }
                );
        }
    }
}
