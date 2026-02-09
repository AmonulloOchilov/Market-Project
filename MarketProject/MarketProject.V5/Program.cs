using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Application.Services;
using MarketProject.V5.Application.UseCases.Customers;
using MarketProject.V5.Application.UseCases.Orders;
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


services.AddSingleton<IRepository<Customer>>(
    new JsonRepository<Customer>(Path.Combine(projectRoot, "Data", "customers.json")));

services.AddSingleton<AddCustomerUseCase>();
services.AddSingleton<GetAllCustomersUseCase>();
services.AddSingleton<UpdateCustomerUseCase>();
services.AddSingleton<DeleteCustomerUseCase>();


services.AddSingleton<IRepository<Order>>(
    new JsonRepository<Order>(Path.Combine(projectRoot, "Data", "orders.json")));

services.AddSingleton<AddOrderUseCase>();
services.AddSingleton<AddItemToOrderUseCase>();
services.AddSingleton<GetAllOrdersUseCase>();
services.AddSingleton<GetOrderDetailsUseCase>();

var provider = services.BuildServiceProvider();

var addProduct = provider.GetRequiredService<AddProductUseCase>();
var getAllProducts = provider.GetRequiredService<GetAllProductsUseCase>();
var deleteProduct = provider.GetRequiredService<DeleteProductUseCase>();
var updateProduct = provider.GetRequiredService<UpdateProductUseCase>();


var addCustomer = provider.GetRequiredService<AddCustomerUseCase>();
var getAllCustomers = provider.GetRequiredService<GetAllCustomersUseCase>();
var updateCustomer = provider.GetRequiredService<UpdateCustomerUseCase>();
var deleteCustomer = provider.GetRequiredService<DeleteCustomerUseCase>();

var addOrder = provider.GetRequiredService<AddOrderUseCase>();
var addItems = provider.GetRequiredService<AddItemToOrderUseCase>();
var getAllOrders = provider.GetRequiredService<GetAllOrdersUseCase>();
var getOrderDetails = provider.GetRequiredService<GetOrderDetailsUseCase>();

ProductMenu productMenu = new ProductMenu(getAllProducts, addProduct, updateProduct, deleteProduct, addCustomer,
    getAllCustomers, updateCustomer, deleteCustomer, addOrder, addItems, getAllOrders, getOrderDetails);
productMenu.ShowMenu();