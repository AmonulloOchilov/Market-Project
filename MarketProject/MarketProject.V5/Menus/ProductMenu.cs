using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Application.UseCases.Products;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Menus;

public class ProductMenu
{
    private readonly GetAllProductsUseCase _getAllProductsUseCase;
    private readonly AddProductUseCase _addProductUseCase;

    public ProductMenu(GetAllProductsUseCase getAllProductsUseCase, AddProductUseCase addProductUseCase)
    {
        _getAllProductsUseCase = getAllProductsUseCase;
        _addProductUseCase = addProductUseCase;
    }
    
    public void ShowMenu()
    {
        
        while (true)
        {
            Console.WriteLine("1 - Add product"); 
            Console.WriteLine("2 - Show products"); 
            Console.WriteLine("3 - Update product"); 
            Console.WriteLine("4 - Delete product"); 
            Console.WriteLine("0 - Back");
            
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Enter product Name: ");
                    string? name = Console.ReadLine();
                    Console.Write("Enter the Quantity: ");
                    double quantity = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter the Expire Date(yyyy:mm:dd) ");
                    DateOnly expireDate = DateOnly.Parse(Console.ReadLine()!);
                    Console.Write("Enter the Price: ");
                    decimal price = decimal.Parse(Console.ReadLine()!);
                    int categoryId = int.Parse(Console.ReadLine()!);
                    var product = new Product
                    {
                        Id = GenerateNextId(),
                        Name = name,
                        Quantity = quantity,
                        ExpireDate = expireDate,
                        PricePerUnit = price,
                        CategoryId = categoryId
                    };
                    _addProductUseCase.Execute(product);
                    break;
                case "2":
                    var products = _getAllProductsUseCase.Execute();
                    foreach (var p in products)
                    {
                        Console.WriteLine($"{p.Id} | {p.Name} | {p.Quantity}");
                    }
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "0": 
                    return;
            }
        }
    }

    private int GenerateNextId()
    {
        var products = _getAllProductsUseCase.Execute();
        if (!products.Any())
        {
            return 1;
        }

        return products.Max(p => p.Id) + 1;
    }
}