using MarketProject.Entities;
using Microsoft.VisualBasic;

namespace MarketProject.Services;

public class OrderService
{
    private readonly string filePath;
    private readonly ProductService productService;
    private readonly CustomerService customerService;
    private string receiptFilePath;
    

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

        string[] customerLines = File.ReadAllLines(customerService.filePath);

        bool customerExists = customerLines.Any(line =>
        {
            string[] parts = line.Split('|');
            return long.Parse(parts[0]) == customerId;
        });
        if (!customerExists)
        {
            Console.WriteLine("Customer not found, try again");
            return;
        }

        Order order = new Order
        {
            Id = File.Exists(filePath) ? File.ReadAllLines(filePath).Length + 1 : 1,
            CustomerId = customerId,
            OrderDate = DateTime.Now
        };
        while (true)
        {
            productService.ViewProducts();
            Console.Write("Enter Product ID to add or Press 0 to finish: ");
            long productId = long.Parse(Console.ReadLine()!);
            if (productId == 0)
            {
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
                Console.WriteLine("Product not found, try again");
                continue;
            }
            Console.Write("Enter Quantity: ");
            double quantity = double.Parse(Console.ReadLine());
            string[] parts = productLine.Split('|');
            decimal price = decimal.Parse(parts[2]);
            double available = double.Parse(parts[3]);
            if (quantity>available)
            {
                Console.WriteLine("Not enough stock, try again");
                continue;
            }

            OrderItem item = new OrderItem()
            {
                Id = DateAndTime.Now.Ticks,
                OrderId = order.Id,
                ProductId = productId,
                Quantity = quantity,
                Amount = price * (decimal)quantity
            };
            order.OrderItems.Add(item);
            Console.WriteLine("Product added");
        }
        Console.WriteLine($"Order Total: {order.TotalAmount}");

        Console.Write("Enter Payment: ");
        order.Payment = decimal.Parse(Console.ReadLine());
        if (order.Payment < order.TotalAmount) 
        {
            Console.WriteLine("Payment not enough, order cancelled");
            return;
        }
        Console.WriteLine($"Change = {order.Change}");
        
        //Save is left
        SaveOrder(order);
        UpdateStock(order);
        GenerateReceipt(order);
    }

    public void SaveOrder(Order order)
    {
        var itemsText = string.Join(";", order.OrderItems.Select(i => $"{i.ProductId}:{i.Quantity}:{i.Amount}"));
        string line =
            $"{order.Id}|{order.CustomerId}|{order.OrderDate}|{order.TotalAmount}|{order.Payment}|{order.Change}|{itemsText}";
        File.AppendAllText(filePath, line+Environment.NewLine);
        Console.WriteLine("Order saved");
    }

    public void UpdateStock(Order order)
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
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No orders found");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            Console.WriteLine("No orders available");
            return;
        }

        Console.WriteLine(
            "{0,-5} {1,-10} {2,-30} {3,-10} {4,-10} {5,-10} {6,-10}",
            "ID", "Customer", "Order Date", "Total", "Payment", "Change", "Items"
        );

        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length >= 7)
            {
                Console.WriteLine(
                    "{0,-5} {1,-10} {2,-30} {3,-10} {4,-10} {5,-10} {6,-10}",
                    parts[0], parts[1], parts[2], parts[3], parts[4], parts[5], parts[6]
                );
            }
        }
    }

    public void ReportDailySales()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No Orders found");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        var today = DateTime.Now.Date;
        int orderCount = 0;
        decimal totalSales = 0;
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length < 7)
            {
                continue;
            }

            DateTime orderDate = DateTime.Parse(parts[2]);
            if (orderDate.Date == today)
            {
                orderCount++;
                totalSales += decimal.Parse(parts[3]);
            }
            
        }

        Console.WriteLine($"Daily Sales Summery ({today:dd.MM.yyyy}):");
        Console.WriteLine($"Total Orders: {orderCount}");
        Console.WriteLine($"Total Sales: {totalSales}");
        
    }

    public void ReportBestSellingProducts()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No orders found.");
            return;
        }

        Dictionary<long, int> productSales = new Dictionary<long, int>();

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length < 7) continue;

            var items = parts[6].Split(';');
            foreach (var item in items)
            {
                var itemParts = item.Split(':');
                if (itemParts.Length < 2) continue;

                long productId = long.Parse(itemParts[0]);
                int quantity = int.Parse(itemParts[1]);

                if (productSales.ContainsKey(productId))
                    productSales[productId] += quantity;
                else
                    productSales[productId] = quantity;
            }
        }

        var products = productService.GetAllProducts();
        Console.WriteLine("Best selling products:");
        foreach (var kv in productSales.OrderByDescending(k => k.Value))
        {
            var product = products.FirstOrDefault(p => p.Id == kv.Key);
            string name = product != null ? product.Name : $"Product {kv.Key}";
            Console.WriteLine($"{name} - {kv.Value} sold");
        }
    }
    public void GenerateReceipt(Order order)
    {
        string receiptFile = "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data/receipts.txt";
        Console.WriteLine("----------------------------");
        string receiptText = "=== RECEIPT ===\n";
        receiptText += $"Order ID: {order.Id}\n";
        receiptText += $"Customer: {order.CustomerId}\n";
        receiptText += $"Date: {order.OrderDate}\n";
        receiptText += "Items:\n";
        

        foreach (var item in order.OrderItems)
        {
            string[] productLine = File.ReadAllLines(productService.filePath)
                .First(line => long.Parse(line.Split('|')[0]) == item.ProductId)
                .Split('|');

            string productName = productLine[1];
            receiptText += $"{productName} x{item.Quantity} = {item.Amount}\n";
        }

        receiptText += $"Total: {order.TotalAmount}\n";
        receiptText += $"Payment: {order.Payment}\n";
        receiptText += $"Change: {order.Change}\n";
        receiptText += "----------------------------\n";
        File.AppendAllText(receiptFile, receiptText);
        Console.WriteLine(receiptText);
    }

    public void ViewAllReceipts()
    {
        string receiptsFile = Path.Combine(
            "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data",
            "receipts.txt"
        );
        if (!File.Exists(receiptsFile))
        {
            Console.WriteLine("No receipts found");
            return;
        }

        string[] allLines = File.ReadAllLines(receiptsFile);
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
}