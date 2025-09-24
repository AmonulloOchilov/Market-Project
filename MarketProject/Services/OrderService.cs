namespace MarketProject.Services;

public class OrderService
{
    private readonly string filePath;
    private readonly ProductService productService;
    private readonly CustomerService customerService;
    

    public OrderService(ProductService productService, CustomerService customerService)
    {
        string dataFolder = "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "orders.txt");
        this.productService = productService;
        this.customerService = customerService;
    }

    public void CreateOrder()
    {
        Console.WriteLine("Create a new order");
        Console.Write("Enter Customer ID: ");
        long customerId = long.Parse(Console.ReadLine()!);
        string[] lines = File.ReadAllLines(customerService.filePath);
        bool customerExists = lines.Any(line =>
        {
            string[] parts = line.Split('|');
            return long.Parse(parts[0]) == customerId;
        });
        if (!customerExists)
        {
            Console.WriteLine("Customer ID not found, try again");
            return;
        }

        List<(long productId, double quantity)> orderItems = new List<(long productId, double quantity)>();
        bool addingProducts = true;
        while (addingProducts)
        {
            productService.ViewProducts();
            Console.Write("Enter product ID to add to order or press 0 to finish: ");
            long productId = long.Parse(Console.ReadLine());
            if (productId == 0)
            {
                addingProducts = false;
                break;
            }


            string[] productLines = File.ReadAllLines(productService.filePath);
            string? productLine = productLines.FirstOrDefault(line =>
            {
                string[] parts = line.Split('|');
                return long.Parse(parts[0]) == productId;
            });
            if (productLine == null)
            {
                Console.WriteLine("Product ID not found. Try again");
                continue;
            }

            Console.Write("Enter quantity: ");
            double quantity = double.Parse(Console.ReadLine());

            string[] parts = productLine.Split('|');
            double availableQuantity = double.Parse(parts[3]);
            if (quantity > availableQuantity)
            {
                Console.WriteLine("Not enough stock is available. Try again");
                continue;
            }

            orderItems.Add((productId, quantity));
            Console.WriteLine("Product added to order");
        }

        decimal total = 0;
        foreach (var (productId, quantity) in orderItems)
        {
            string[] productLines = File.ReadAllLines(productService.filePath);
            string productLine = productLines.First(line => long.Parse(line.Split('|')[0]) == productId);
            string[] parts = productLine.Split('|');
            decimal price = decimal.Parse(parts[2]);
            total += price * (decimal)quantity;
        }
        
        Console.WriteLine($"Order total: {total}");
        
        long orderId;
        if (File.Exists(filePath))
        {
            int lineCount = File.ReadAllLines(filePath).Length;
            orderId = lineCount + 1;
        }
        else
        {
            orderId = 1;
        }

        List<string> orderItemsFormatted = new List<string>();
        foreach (var item in orderItems)
        {
            string formatted = $"{item.productId}:{item.quantity}";
            orderItemsFormatted.Add(formatted);
        }
        string itemString = string.Join(",", orderItemsFormatted);
        string orderLine = $"{orderId}|{customerId}|{itemString}|{total}";
        File.AppendAllText(filePath, orderLine + Environment.NewLine);
        Console.WriteLine("Order saved successfully");

        var productLiness = File.ReadAllLines(productService.filePath).ToList();
        for (int i = 0; i < productLiness.Count; i++)
        {
            string[] parts = productLiness[i].Split('|');
            long prodId = long.Parse(parts[0]);
            
            var orderedItem = orderItems.FirstOrDefault(x => x.productId == prodId);
            if (orderedItem != default)
            {
                double currentStock = double.Parse(parts[3]);
                double newStock = currentStock - orderedItem.quantity;
                parts[3] = newStock.ToString();
                productLiness[i] = string.Join("|", parts);
            }
        }
        
        File.WriteAllLines(productService.filePath, productLiness);
        Console.WriteLine("Stock updated successfully!");
    }
    public void ViewOrders()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No orders found");
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            Console.WriteLine("No orders available");
        }

        Console.WriteLine("Order ID\tCustomer ID\tItems\t\tTotal Amount");
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length >= 4)
            {
                Console.WriteLine($"{parts[0]}\t\t{parts[1]}\t\t{parts[2]}\t\t{parts[3]}");
            }
        }
    }
    
}