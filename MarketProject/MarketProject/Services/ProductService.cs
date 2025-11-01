using MarketProject.Entities;
using System.Text.Json;
namespace MarketProject.Services;

public class ProductService
{
    public readonly string filePath;

    public ProductService()
    {
        string dataFolder = "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "products.json");
    }
    private List<Product> LoadProducts()
    {
        if (!File.Exists(filePath))
        {
            return new List<Product>();
        }
            

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Product>();
        }
        
        var result = JsonSerializer.Deserialize<List<Product>>(json);
        if (result != null)
        {
            return result;
        }
        else
        {
            return new List<Product>();
        }
    }
    
    private void SaveProducts(List<Product> products)
    {
        string json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public void AddProduct(long id, string name, double quantity, DateOnly expireDate, decimal price, long categoryId)
    {
        var products = LoadProducts();
        var product = new Product()
        {
            Id = id,
            Name = name,
            Quantity = quantity,
            ExpireDate = expireDate,
            PricePerUnit = price,
            CategoryID = categoryId,
            
        };
        products.Add(product);
        SaveProducts(products);
        Console.WriteLine("Product saved successfully!");
    }

    public void ViewProducts()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No products found");
            return;
        }

        string json = File.ReadAllText(filePath);
        var products = LoadProducts();
        if (string.IsNullOrWhiteSpace(json) || products.Count == 0)
        {
            Console.WriteLine("No products available");
        }
        
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id} - Name: {product.Name} - Quantity: {product.Quantity} - Expiration Date: {product.ExpireDate} - Price: {product.PricePerUnit} - Category ID: {product.CategoryID}");
        }
    }

    public void EditProduct()
    {
        var products = LoadProducts();
        Console.Write("Enter product ID: ");
        long productId = long.Parse(Console.ReadLine()!);
        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found");
            return;
        }
        
        Console.Write($"Current Name: {product.Name}, enter new name or press Enter to keep: ");
        string newName = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(newName))
        {
            product.Name = newName;
        }

        Console.Write($"Current Price: {product.PricePerUnit}, enter New Price or press Enter to keep: ");
        string newPrice = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(newPrice))
        {
            product.PricePerUnit = Convert.ToDecimal(newPrice);
        }
        
        Console.Write($"Current Quantity: {product.Quantity}, enter New Quantity or press Enter to keep: ");
        string newQuantity = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(newQuantity))
        {
            product.Quantity = Convert.ToDouble(newQuantity);
        }

        Console.Write($"Current Category ID: {product.CategoryID}, enter New Category ID or press Enter to keep: ");
        string newCategoryId = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(newCategoryId))
        {
            product.CategoryID = Convert.ToInt64(newCategoryId);
        }
        
        SaveProducts(products);
        Console.WriteLine("Product saved");
    }

    public void DeleteProduct()
    {
        var products = LoadProducts();
        Console.Write("Enter product ID: ");
        long productId = long.Parse(Console.ReadLine()!);
        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found");
            return;
        }

        if (product != null)
        {
            products.Remove(product);
            Console.WriteLine($"Product {product.Name} is removed");
        }
        SaveProducts(products);
    }

    public void ReportsLowStock()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No products found");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        Console.WriteLine("Low Stock Products (Less then 5):");
        Console.WriteLine("ID\tName\tQuantity");
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 5)
            {
                long id = long.Parse(parts[0]);
                string name = parts[1];
                double quantity = double.Parse(parts[3]);
                if (quantity < 5) 
                {
                    Console.WriteLine($"{id}\t{name}\t\t{quantity}");
                }
            }
        }
    }
    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();
        if (!File.Exists(filePath))
        {
            return products;
        }

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length == 5)
            {
                products.Add(new Product
                {
                    Id = long.Parse(parts[0]),
                    Name = parts[1],
                    PricePerUnit = decimal.Parse(parts[2]),
                    Quantity = double.Parse(parts[3]),
                    CategoryID = long.Parse(parts[4])
                });
            }
        }
        return products;
    }

    public void SaveAllProducts(List<Product> products)
    {
        var lines = products.Select(p => $"{p.Id}|{p.Name}|{p.PricePerUnit}|{p.Quantity}|{p.CategoryID}");
        File.WriteAllLines(filePath,lines);
    }

}