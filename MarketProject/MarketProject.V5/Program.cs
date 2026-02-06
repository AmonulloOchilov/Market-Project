using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Application.Services;
using MarketProject.V5.Application.UseCases.Products;
using MarketProject.V5.Domain;
using MarketProject.V5.Infrastructure.Repositories;
using MarketProject.V5.Menus;
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
services.AddSingleton<DeleteProductUseCase>();
services.AddSingleton<UpdateProductUseCase>();

var provider = services.BuildServiceProvider();

var addProduct = provider.GetRequiredService<AddProductUseCase>();
var getAllProducts = provider.GetRequiredService<GetAllProductsUseCase>();
var deleteProduct = provider.GetRequiredService<DeleteProductUseCase>();
var updateProduct = provider.GetRequiredService<UpdateProductUseCase>();

ProductMenu productMenu = new ProductMenu(getAllProducts, addProduct, updateProduct, deleteProduct);
productMenu.ShowMenu();