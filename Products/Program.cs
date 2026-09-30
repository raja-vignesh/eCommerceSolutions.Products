


using Products.Api.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddInfra(builder.Configuration).AddCore();

var app = builder.Build();

await DbSeeder.Seed(app.Services);
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
