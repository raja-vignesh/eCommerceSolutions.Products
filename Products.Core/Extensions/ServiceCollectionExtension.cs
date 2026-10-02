
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.Core.MappingProfiles;
using Products.Core.Validators;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
        serviceCollection.AddValidatorsFromAssemblyContaining<GetProductsQueryValidator>();
        serviceCollection.AddAutoMapper(config => config.AddProfile<ProductsProfile>());
        return serviceCollection;
    }
}
