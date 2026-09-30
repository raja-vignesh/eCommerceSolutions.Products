using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infra;
using System.Text.Json;

namespace Products.Api.Seeder;

public static class DbSeeder
{
    /// <summary>
    /// Products seeder file
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async static Task Seed(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        if (context.Database != null) { 
            if (!await context.Products.AnyAsync())
            {
                var jsonPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "SeederJson",
                    "products.json");
                var jsonData = await File.ReadAllTextAsync(jsonPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var products = JsonSerializer.Deserialize<List<Product>>(jsonData, options);
                
                if (products.Any() == true) { 
                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }
            } 
        }
    }

}
