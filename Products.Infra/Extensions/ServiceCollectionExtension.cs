
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Domain.RepositoryContracts;
using Products.Infra;
using Products.Infra.Repository;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfra(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).EnableSensitiveDataLogging());
        serviceCollection.AddScoped<IProductsRepository,ProductsRepository>();
        return serviceCollection;
    }
}
