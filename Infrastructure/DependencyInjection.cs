using Application.Common.Interfaces;
using Application.Common.Service;
using Application.Product.Interface;
using Application.Product.Service;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("HMDatabase"),
                b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<IApplicationDBContext>(provider => provider.GetService<ApplicationDBContext>());
            services.AddScoped<IColourCRUDOperations, ColourCRUDOperations>();
            services.AddScoped<ISizeCRUDOperations, SizeCRUDOperations>();
            services.AddScoped<IProductCRUDOperations, ProductCRUDOperations>();
            services.AddScoped<IExternalAPIService, ExternalAPIService>();
            return services;
        }
    }
}
