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
        var products = LoadProducts();

        var lowStock = products.Where(p => p.Quantity < 5).ToList();
        if (lowStock.Count == 0)
        {
            Console.WriteLine("No low stock products.");
            return;
        }

        Console.WriteLine("Low Stock Products (less than 5):");
        Console.WriteLine("ID\tName\tQuantity");

        foreach (var p in lowStock)
        {
            Console.WriteLine($"{p.Id}\t{p.Name}\t{p.Quantity}");
        }
    }

    public List<Product> GetAllProducts()
    {
        return LoadProducts();
    }
    public void SaveAllProducts(List<Product> products)
    {
        SaveProducts(products);
    }
}