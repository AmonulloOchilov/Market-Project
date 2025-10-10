using MarketProject.Entities;
using MarketProject.Services;

ProductService productService = new ProductService();
CustomerService customerService = new CustomerService();

OrderService orderService = new OrderService(productService, customerService);
Console.WriteLine("Welcome to Market");

void ShowMenu()
{
    Console.WriteLine("Main Menu:");
    Console.WriteLine("1. Add Product");
    Console.WriteLine("2. View Products");
    Console.WriteLine("3. Edit Product");
    Console.WriteLine("4. Delete Product");
    Console.WriteLine("5. Add Customer");
    Console.WriteLine("6. View Customers");
    Console.WriteLine("7. Edit Customer");
    Console.WriteLine("8. Delete Customer");
    Console.WriteLine("9. Create Order");
    Console.WriteLine("10. View Orders");
    Console.WriteLine("11. Reports");
    Console.WriteLine("12. Exit");
    Console.Write("Select an option (1-12): ");

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
            productService.EditProduct();
            break;
        case "4":
            productService.DeleteProduct();
            break;
        case "5":
            AddCustomerMenu();
            break;
        case "6":
            customerService.ViewCustomers();
            break;
        case "7":
            customerService.EditCustomer();
            break;
        case "8":
            customerService.DeleteCustomer();
            break;
        case "9":
            orderService.CreateOrder();
            break;
        case "10":
            orderService.ViewOrders();
            break;
        case "11":
            ReportsMenu();
            break;
        case "12":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid Option. Please select (1-12)");
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

void ReportsMenu()
{
    Console.WriteLine("Reports Menu:");
    Console.WriteLine("1. Low stock products");
    Console.WriteLine("2. Daily sales summery");
    Console.WriteLine("3. Best selling products");
    Console.WriteLine("4. Receipts");
    Console.WriteLine("5. Back to Main Menu");
    Console.WriteLine("Select an option (1-5): ");
    string? choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            productService.ReportsLowStock();
            break;
        case "2":
            orderService.ReportDailySales();
            break;
        case "3":
            orderService.ReportBestSellingProducts();
            break;
        case "4": 
            orderService.ViewAllReceipts();
            break;
        case "5":
            return;
        default:
            Console.WriteLine("Invalid Option. Try again");
            break;
    }
    //Pr checking

}