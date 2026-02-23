using ECommerece.Application.Mappers;
using ECommerece.Infrastructure.AppDbContext;
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

            System.Windows.Forms.Application.Run(host.Services.GetRequiredService<Form1>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Presentation
                    services.AddTransient<Form1>();

                    // Application Layer Services
                    // services.AddScoped<IProductService, ProductService>();

                    // Infrastructure Layer
                    services.AddDbContext<ECommerceDbContext>(options =>
                                options.UseSqlServer(
                                    context.Configuration.GetConnectionString("DefaultConnection"),
                                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                                ));
                });
                }
    }
    }