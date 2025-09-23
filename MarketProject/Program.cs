using MarketProject.Entities;
using MarketProject.Services;

ProductService productService = new ProductService();
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



// Customer customer1 = new Customer();
// customer1.Id = 1;
// customer1.Name = "Amin";
// customer1.Surname = "Ochilov";
// customer1.Email = "amonullo436@gmail.com";
// customer1.PhoneNumber = "+992987846169";
//
// Product product1 = new Product();
// product1.Id = 1;
// product1.Name = "Milk";
// product1.Quantity = 10;
// product1.ExpireDate = new DateTime(2025, 09, 20);
// product1.PricePerUnit = 12.5m;
// product1.CategoryID = 1; // 1 is Dairy
//
// Product product2 = new Product();
// product2.Id = 2;
// product2.Name = "Bread";
// product2.Quantity = 20;
// product2.ExpireDate = new DateTime(2025, 09, 20);
// product2.PricePerUnit = 3.0m;
// product2.CategoryID = 2; // 2 is Bakery
//
//
// Order order1 = new Order();
// order1.Id = 1;
// order1.CustomerId = 1;
// order1.OrderDate = new DateTime(2025, 09, 13);
// order1.AddItem(product1, 9);
// order1.AddItem(product2, 2);
// Console.WriteLine($"Order total: {order1.TotalAmount}"); // 25
//
// Console.WriteLine($"Product left: {product1.Quantity}, {product2.Quantity}");