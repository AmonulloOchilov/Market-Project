using MarketProject.Entities;
using MarketProject.Services;

ProductService productService = new ProductService();
CustomerService customerService = new CustomerService();
Console.WriteLine("Welcome to Market");

void ShowMenu()
{
    Console.WriteLine("Main Menu");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. View Products");
    Console.WriteLine("3. Add Customer");
    Console.WriteLine("4. View Customers");
    Console.WriteLine("5. Create Order");
    Console.WriteLine("6. View Orders");
    Console.WriteLine("7. Exit");
    Console.WriteLine("Main Menu");
    Console.Write("Select an option (1-7): ");

}
while (true)
{
    ShowMenu();
    string choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            AddProductMenu();
            break;
        case "2":
            productService.ViewProducts();
            break;
        case "3":
            AddCustomerMenu();
            break;
        case "4":
            customerService.ViewCustomers();
            break;
        case "7":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid Option. Please select (1-7)");
            break;
    }
}

void AddProductMenu()
{
    Console.WriteLine("Add new Product");
    try
    {
        Console.Write("Enter Product ID: ");
        long id = long.Parse(Console.ReadLine());
            
        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine();
            
        Console.Write("Enter Price: ");
        decimal price = decimal.Parse(Console.ReadLine());
            
        Console.Write("Enter Quantity: ");
        double quantity = double.Parse(Console.ReadLine());
            
        Console.Write("Enter Category ID: ");
        long categoryId = long.Parse(Console.ReadLine());
            
        productService.AddProduct(id, name, price, quantity, categoryId);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    
}

void AddCustomerMenu()
{
    Console.WriteLine("Add new Customer");
    try
    {
        Console.Write("Enter Customer ID: ");
        long id = long.Parse(Console.ReadLine()!);
            
        Console.Write("Enter Customer Name: ");
        string? name = Console.ReadLine();
            
        Console.Write("Enter Customer Surname: ");
        string? surname = Console.ReadLine();

        Console.Write("Enter Customer Email: ");
        string? email = Console.ReadLine();

        Console.Write("Enter Customer Phone Number: ");
        string? phoneNumber = Console.ReadLine();
        customerService.AddCustomer(id, name, surname, email, phoneNumber);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}