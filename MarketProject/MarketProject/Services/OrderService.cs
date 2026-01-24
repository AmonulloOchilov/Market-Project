using System.Text.Json;
using MarketProject.Entities;
using Microsoft.VisualBasic;

namespace MarketProject.Services;

public class OrderService
{
    private readonly string filePath;
    private readonly ProductService productService;
    private readonly CustomerService customerService;
    private readonly string? receiptFilePath;
    

    public OrderService(ProductService productService, CustomerService customerService)
    {
        string dataFolder = "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "orders.json");
        receiptFilePath = Path.Combine(dataFolder, "receipts.txt");
        this.productService = productService;
        this.customerService = customerService;
    }

    private List<Order> LoadOrders()
    {
        if (!File.Exists(filePath))
        {
            return new List<Order>();
        }

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Order>();
        }

        var result = JsonSerializer.Deserialize<List<Order>>(json);
        if (result != null)
        {
            return result;
        }
        else
        {
            return new List<Order>();
        }
    }

    private void SaveOrder(List<Order> orders)
    {
        string json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void CreateOrder()
    {
        Console.WriteLine("Create a new order");
        Console.Write("Enter Customer ID: ");
        long customerId = long.Parse(Console.ReadLine()!);
        var orders = LoadOrders();
        long nextId;
        if (orders.Count == 0)
        {
            nextId = 1;
        }
        else
        {
            nextId = orders.Max(o => o.Id) + 1;
        }
        Order order = new Order
        {
            Id = nextId,
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            Status = "Pending"
        };
        
        Console.WriteLine("Order Status: " + order.Status);
        
        var products = productService.GetAllProducts();
        
        while (true)
        {
            productService.ViewProducts();
            Console.Write("Enter Product ID to add or press 0 to finish: ");
            long productId = long.Parse(Console.ReadLine()!);
            if (productId == 0)
            {
                break;
            }

            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                Console.WriteLine("Product Not found try again");
                continue;
            }

            Console.Write("Enter Quantity: ");
            double quantity = double.Parse(Console.ReadLine()!);
            if (quantity > product.Quantity) 
            {
                Console.WriteLine($"Not enough stock. Only {product.Quantity} left.");
                continue;
            }

            product.Quantity -= quantity;

            OrderItem item = new OrderItem()
            {
                Id = order.OrderItems.Count+1,
                OrderId = order.Id,
                ProductId = productId,
                Quantity = quantity,
                Amount = product.PricePerUnit * (decimal)quantity
            };
            order.OrderItems.Add(item);
            Console.WriteLine($"{product.Name} added to order (Reserved: {quantity})");
        }
        
        Console.WriteLine($"\nOrder Total: {order.TotalAmount}");
        Console.WriteLine("Confirm Order? Yes/No");
        string confirm = Console.ReadLine()!;
        if (confirm == "No")
        {
            order.Status = "Canceled";
            Console.WriteLine("Order canceled with Status: " + order.Status);
            return;
        }
        
        Console.Write("Enter Payment: ");
        decimal payment = decimal.Parse(Console.ReadLine()!);
        if (payment < order.TotalAmount) 
        {
            Console.WriteLine("Payment not enough, order cancelled");
            return;
        }

        order.Payment = payment;
        order.Status = "Confirmed";
        Console.WriteLine($"Change = {order.Change}");
        orders.Add(order);
        SaveOrder(orders);
        productService.SaveAllProducts(products);
        
        GenerateReceipt(order);
        Console.WriteLine("Order confirmed and stock updated\n");
    }
    
    private void UpdateStock(Order order)
    {
        var productLines = File.ReadAllLines(productService.filePath).ToList();
        for (int i = 0; i < productLines.Count; i++)
        {
            string[] parts = productLines[i].Split('|');
            long prodId = long.Parse(parts[0]);
            var orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == prodId);
            if (orderItem != null)
            {
                double currentStock = double.Parse(parts[3]);
                double newStock = currentStock - orderItem.Quantity;
                parts[3] = newStock.ToString();
                productLines[i] = string.Join("|", parts);
            }
        }
        File.WriteAllLines(productService.filePath, productLines);
        Console.WriteLine("Stock updated successfully");
    }
    public void ViewOrders()
    {
        var orders = LoadOrders();

        if (orders.Count == 0)
        {
            Console.WriteLine("No orders available");
            return;
        }

        foreach (var order in orders)
        {
            Console.WriteLine($"\nOrder ID: {order.Id}");
            Console.WriteLine($"Customer ID: {order.CustomerId}");
            Console.WriteLine($"Order Date: {order.OrderDate}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine("Items:");

            if (order.OrderItems.Count == 0)
            {
                Console.WriteLine("  No items");
            }
            else
            {
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"  Product ID: {item.ProductId} | Quantity: {item.Quantity} | Amount: {item.Amount}");
                }
            }

            Console.WriteLine($"Total: {order.TotalAmount}");
            Console.WriteLine($"Payment: {order.Payment}");
            Console.WriteLine($"Change: {order.Change}");
        }
    }


    public void ReportDailySales()
    {
        var orders = LoadOrders();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found");
            return;
        }
        DateTime today = DateTime.Now.Date;
        List<Order> confirmedTodayOrders = orders
            .Where(o => o.OrderDate.Date == today && o.Status == "Confirmed").ToList();
        int orderCount = confirmedTodayOrders.Count;
        decimal totalSales = confirmedTodayOrders.Sum(o => o.TotalAmount);
        
        Console.WriteLine($"\nDaily Sales Summary ({today:dd.MM.yyyy}):");
        Console.WriteLine($"Total Orders: {orderCount}");
        Console.WriteLine($"Total Sales: {totalSales}");
    }


    public void ReportBestSellingProducts()
    {
        var orders = LoadOrders().Where(o => o.Status == "Confirmed").ToList();
        if (orders.Count == 0)
        {
            Console.WriteLine("No confirmed orders found.");
            return;
        }
        var productSales = new Dictionary<long, double>();

        foreach (var order in orders)
        {
            foreach (var item in order.OrderItems)
            {
                if (!productSales.ContainsKey(item.ProductId))
                    productSales[item.ProductId] = 0;

                productSales[item.ProductId] += item.Quantity;
            }
        }
        var products = productService.GetAllProducts();

        Console.WriteLine("\nBest Selling Products:");
        foreach (var kv in productSales.OrderByDescending(k => k.Value))
        {
            var product = products.FirstOrDefault(p => p.Id == kv.Key);
            string name = product?.Name ?? $"Product {kv.Key}";
            Console.WriteLine($"{name} - {kv.Value} sold");
        }
    }


    private void GenerateReceipt(Order order)
    {
        var products = productService.GetAllProducts();
        string receiptText = "\n RECEIPT \n";
        receiptText += $"Order ID: {order.Id}\n";
        receiptText += $"Customer: {order.CustomerId}\n";
        receiptText += $"Date: {order.OrderDate}\n";
        receiptText += "Items:\n";

        foreach (var item in order.OrderItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            string productName = product?.Name ?? "Unknown";
            receiptText += $"{productName}  Quantity: {item.Quantity}  Price: {item.Amount}\n";
        }

        receiptText += $"Total: {order.TotalAmount}\n";
        receiptText += $"Payment: {order.Payment}\n";
        receiptText += $"Change: {order.Change}";

        File.AppendAllText(receiptFilePath, receiptText);
        Console.WriteLine(receiptText);
    }
    

    public void ViewAllReceipts()
    {
        string receiptFile = Path.Combine(
            "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data",
            "receipts.txt"
        );

        if (!File.Exists(receiptFile))
        {
            Console.WriteLine("No receipts found.");
            return;
        }

        string[] allLines = File.ReadAllLines(receiptFile);

        if (allLines.Length == 0)
        {
            Console.WriteLine("No receipts available.");
            return;
        }

        Console.WriteLine("===== All Receipts =====");
        foreach (var line in allLines)
        {
            Console.WriteLine(line);
        }
    }


    public void EditOrCancelOrder()
    {
        var orders = LoadOrders();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        List<string> lines = File.ReadAllLines(filePath).ToList();

        Console.Write("Enter Order ID to Edit or Cancel: ");
        long orderId = long.Parse(Console.ReadLine()!);

        var orderLine = orders.FirstOrDefault(o => o.Id == orderId);
        if (orderLine == null)
        {
            Console.WriteLine("Order not found");
            return;
        }

        Console.Write("Do you want to Edit or Cancel this order? (Edit/Cancel): ");
        string choice = Console.ReadLine()!.Trim().ToLower();

        var products = productService.GetAllProducts();
        foreach (var item in orderLine.OrderItems)
        {
            var prod = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (prod != null)
            {
                prod.Quantity += item.Quantity;
            }
        }

        productService.SaveAllProducts(products);
        
        File.WriteAllLines(filePath, lines);

        if (choice == "cancel")
        {
            Console.WriteLine("Order successfully canceled.");
        }
        else if (choice == "edit")
        {
            orders.Remove(orderLine);
            SaveOrder(orders);
            Console.WriteLine("Old order removed. Now create a new order:");
            CreateOrder();
            return;
        }
        else
        {
            Console.WriteLine("Invalid choice.");
            return;
        }
    }


    public void ViewCustomerOrderHistory()
    {
        var orders = LoadOrders();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found.");
            return;
        }

        Console.Write("Enter Customer ID: ");
        long customerId = long.Parse(Console.ReadLine()!);

        var products = productService.GetAllProducts();
        var custOrders = orders.Where(o => o.CustomerId == customerId).ToList();

        Console.WriteLine($"\nOrder History for Customer {customerId}");
        bool foundAny = false;
        foreach (var order in custOrders)
        {
            Console.WriteLine($"\nOrder ID: {order.Id}");
            Console.WriteLine($"Date: {order.OrderDate}");
            Console.WriteLine($"Status: {order.Status}");
            Console.WriteLine("Items:");
            foreach (var item in order.OrderItems)
            {
                var prod = products.FirstOrDefault(p => p.Id == item.ProductId);
                string name = prod?.Name ?? $"Product {item.ProductId}";
                Console.WriteLine($" - {name} x{item.Quantity}  = {item.Amount}");
            }

            Console.WriteLine($"Total Amount: {order.TotalAmount}");
            foundAny = true;
        }
        if (!foundAny)
        {
            Console.WriteLine("Customer has no order history.");
        }
    }

}