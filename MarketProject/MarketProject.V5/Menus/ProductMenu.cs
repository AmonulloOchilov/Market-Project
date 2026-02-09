using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Application.UseCases.Customers;
using MarketProject.V5.Application.UseCases.Orders;
using MarketProject.V5.Application.UseCases.Products;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Menus;

public class ProductMenu
{
    private readonly GetAllProductsUseCase _getAllProductsUseCase;
    private readonly AddProductUseCase _addProductUseCase;
    private readonly UpdateProductUseCase _updateProductUseCase;
    private readonly DeleteProductUseCase _deleteProductUseCase;

    private readonly AddCustomerUseCase _addCustomerUseCase;
    private readonly GetAllCustomersUseCase _getAllCustomersUseCase;
    private readonly UpdateCustomerUseCase _updateCustomerUseCase;
    private readonly DeleteCustomerUseCase _deleteCustomerUseCase;

    private readonly AddOrderUseCase _addOrderUseCase;

    public ProductMenu(GetAllProductsUseCase getAllProductsUseCase, AddProductUseCase addProductUseCase,
        UpdateProductUseCase updateProductUseCase, DeleteProductUseCase deleteProductUseCase,
        AddCustomerUseCase addCustomerUseCase, GetAllCustomersUseCase getAllCustomersUseCase,
        UpdateCustomerUseCase updateCustomerUseCase, DeleteCustomerUseCase deleteCustomerUseCase, AddOrderUseCase addOrderUseCase)
    {
        _getAllProductsUseCase = getAllProductsUseCase;
        _addProductUseCase = addProductUseCase;
        _updateProductUseCase = updateProductUseCase;
        _deleteProductUseCase = deleteProductUseCase;

        _addCustomerUseCase = addCustomerUseCase;
        _getAllCustomersUseCase = getAllCustomersUseCase;
        _updateCustomerUseCase = updateCustomerUseCase;
        _deleteCustomerUseCase = deleteCustomerUseCase;

        _addOrderUseCase = addOrderUseCase;
    }
    
    public void ShowMenu()
    {
        
        while (true)
        {
            Console.WriteLine("1 - Add product"); 
            Console.WriteLine("2 - Show products"); 
            Console.WriteLine("3 - Update product"); 
            Console.WriteLine("4 - Delete product");
            Console.WriteLine("5 - Add customer");
            Console.WriteLine("6 - Show customers");
            Console.WriteLine("7 - Update customer");
            Console.WriteLine("8 - Delete customer");
            Console.WriteLine("9 - Create order");
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
                    foreach (var p in products)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-10} {3,-15} {4,-15} {5, -10}",
                            $"{p.Id}", $"{p.Name}", $"{p.Quantity}", $"{p.ExpireDate}", $"{p.PricePerUnit}",
                            $"{p.CategoryId}");
                    }
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
                    break;
                }
                case "5":
                {
                    Console.Write("Enter customer Name: ");
                    string customerName = Console.ReadLine()!;
                    Console.Write("Enter customer Surname: ");
                    string customerSurname = Console.ReadLine()!;
                    Console.Write("Enter customer Email: ");
                    string customerEmail = Console.ReadLine()!;
                    Console.Write("Enter customer Phone Number: ");
                    string customerPhoneNumber = Console.ReadLine()!;
                    var customer = new Customer
                    {
                        Name = customerName,
                        Surname = customerSurname,
                        Email = customerEmail,
                        PhoneNumber = customerPhoneNumber
                    };
                    _addCustomerUseCase.Execute(customer);
                    break;
                }
                case "6":
                {
                    var customers = _getAllCustomersUseCase.Execute();
                    foreach (var c in customers)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-30} {4,-25}",
                            $"{c.Id}", $"{c.Name}", $"{c.Surname}", $"{c.Email}", $"{c.PhoneNumber}");
                    }
                    break;
                }
                case "7":
                {
                    var customers = _getAllCustomersUseCase.Execute();
                    foreach (var c in customers)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-30} {4,-25}",
                            $"{c.Id}", $"{c.Name}", $"{c.Surname}", $"{c.Email}", $"{c.PhoneNumber}");
                    }
                    
                    Console.Write("Enter customer ID: ");
                    int customerId = int.Parse(Console.ReadLine()!);

                    Console.Write("New Name: ");
                    string? name = Console.ReadLine();

                    Console.Write("New Surname: ");
                    string? surname = Console.ReadLine();

                    Console.Write("New Email: ");
                    string? email = Console.ReadLine();

                    Console.Write("New Phone: ");
                    string? phone = Console.ReadLine();
                    var updatedCustomer = new Customer()
                    {
                        Id = customerId,
                        Name = name,
                        Surname = surname,
                        Email = email,
                        PhoneNumber = phone
                    };
                    
                    var updated = _updateCustomerUseCase.Execute(updatedCustomer);
                    if (!updated)
                    {
                        Console.WriteLine("Customer not found");
                    }
                    break;
                }
                case "8":
                {
                    var customers = _getAllCustomersUseCase.Execute();
                    foreach (var c in customers)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-30} {4,-25}",
                            $"{c.Id}", $"{c.Name}", $"{c.Surname}", $"{c.Email}", $"{c.PhoneNumber}");
                    }

                    Console.Write("Enter customer ID: ");
                    int customerId = int.Parse(Console.ReadLine()!);
                    var deleted = _deleteCustomerUseCase.Execute(customerId);
                    if (!deleted)
                    {
                        Console.WriteLine("Customer not found");
                    }
                    break;
                }
                case "9":
                {
                    var customers = _getAllCustomersUseCase.Execute();
                    Console.Write("Enter customer ID: ");
                    int customerId = int.Parse(Console.ReadLine()!);
                    var customer = customers.FirstOrDefault(c => c.Id == customerId);
                    if (customer == null)
                    {
                        Console.WriteLine("Customer not found");
                        continue;
                    }

                    var products = _getAllProductsUseCase.Execute();
                    foreach (var p in products)
                    {
                        Console.WriteLine("{0,-5} {1,-15} {2,-10} {3,-15} {4,-15} {5, -10}",
                            $"{p.Id}", $"{p.Name}", $"{p.Quantity}", $"{p.ExpireDate}", $"{p.PricePerUnit}",
                            $"{p.CategoryId}");
                    }

                    Console.Write("Enter product ID: ");
                    int productId = int.Parse(Console.ReadLine()!);

                    // Finish the MENU!!!
                    // _addOrderUseCase.Execute();
                    break;
                }
                    
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