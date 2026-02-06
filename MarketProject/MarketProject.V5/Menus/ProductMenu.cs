using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Application.UseCases.Products;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Menus;

public class ProductMenu
{
    private readonly GetAllProductsUseCase _getAllProductsUseCase;
    private readonly AddProductUseCase _addProductUseCase;
    private readonly UpdateProductUseCase _updateProductUseCase;
    private readonly DeleteProductUseCase _deleteProductUseCase;

    public ProductMenu(GetAllProductsUseCase getAllProductsUseCase, AddProductUseCase addProductUseCase,
        UpdateProductUseCase updateProductUseCase, DeleteProductUseCase deleteProductUseCase)
    {
        _getAllProductsUseCase = getAllProductsUseCase;
        _addProductUseCase = addProductUseCase;
        _updateProductUseCase = updateProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;
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
            Console.Write("Select an option: ");
            
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                {
                    Console.Write("Enter product Name: ");
                    string? name = Console.ReadLine();
                    Console.Write("Enter the Quantity: ");
                    double quantity = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter the Expire Date(yyyy-mm-dd): ");
                    DateOnly expireDate = DateOnly.Parse(Console.ReadLine()!);
                    Console.Write("Enter the Price: ");
                    decimal price = decimal.Parse(Console.ReadLine()!);
                    Console.Write("Enter the Category ID: ");
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
                }

                case "2":
                {
                    var products = _getAllProductsUseCase.Execute();
                    foreach (var p in products)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-10} {3,-15} {4,-15} {5, -10}",
                            $"{p.Id}", $"{p.Name}", $"{p.Quantity}", $"{p.ExpireDate}", $"{p.PricePerUnit}",
                            $"{p.CategoryId}");
                    }

                    break;
                }
                case "3":
                {
                    var products = _getAllProductsUseCase.Execute();
                    Console.Write("Enter product ID: ");
                    int productId = int.Parse(Console.ReadLine()!);
                    var product = products.FirstOrDefault(p => p.Id == productId);
                    if (product == null)
                    {
                        Console.WriteLine("Product not found");
                        continue;
                    }
                    Console.Write("Enter new product Name: ");
                    product.Name = Console.ReadLine();
                    Console.Write("Enter the new Quantity: ");
                    product.Quantity = double.Parse(Console.ReadLine()!);
                    Console.Write("Enter the new Expire Date(yyyy-mm-dd): ");
                    product.ExpireDate = DateOnly.Parse(Console.ReadLine()!);
                    Console.Write("Enter the new Price: ");
                    product.PricePerUnit = decimal.Parse(Console.ReadLine()!);
                    Console.Write("Enter the new Category ID: ");
                    product.CategoryId = int.Parse(Console.ReadLine()!);
                    
                    _updateProductUseCase.Execute(product);
                    break;
                }
                case "4":
                {
                    var products = _getAllProductsUseCase.Execute();
                    
                    foreach (var p in products)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-10} {3,-15} {4,-15} {5, -10}",
                            $"{p.Id}", $"{p.Name}", $"{p.Quantity}", $"{p.ExpireDate}", $"{p.PricePerUnit}",
                            $"{p.CategoryId}");
                    }
                    
                    Console.Write("Enter Product ID: ");
                    int productId = int.Parse(Console.ReadLine()!);
                    var product = products.FirstOrDefault(p => p.Id == productId);
                    
                    if (product == null)
                    {
                        Console.WriteLine("Product not found");
                        continue;
                    }
                    _deleteProductUseCase.Execute(productId);
                    
                }
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