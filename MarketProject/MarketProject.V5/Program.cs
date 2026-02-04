using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Application.Services;
using MarketProject.V5.Application.UseCases.Products;
using MarketProject.V5.Domain;
using MarketProject.V5.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var projectRoot =
    Directory.GetParent(Directory.GetCurrentDirectory())!
        .Parent!
        .Parent!
        .FullName;

var dataPath = Path.Combine(projectRoot, "Data", "products.json");

services.AddSingleton<IRepository<Product>>(new JsonRepository<Product>(dataPath));

services.AddSingleton<AddProductUseCase>();
services.AddSingleton<GetAllProductsUseCase>();

var provider = services.BuildServiceProvider();

var getAllProducts = provider.GetRequiredService<GetAllProductsUseCase>();
var addProduct = provider.GetRequiredService<AddProductUseCase>();

addProduct.Execute(new Product
{
    Id = 2,
    Name = "Banana",
    Quantity = 8,
    PricePerUnit = 1.2m,
    ExpireDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
    CategoryId = 1
});

var products = getAllProducts.Execute();

foreach (var p in products)
{
    Console.WriteLine($"[UC] {p.Name} | {p.Quantity} | {p.PricePerUnit}");
}