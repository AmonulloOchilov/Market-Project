using MarketProject.Entities;

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

        filePath = Path.Combine(dataFolder, "products.txt");
    }

    public void AddProduct(long id, string name, decimal price, double quantity, long categoryId)
    {
        var product = new Product()
        {
            Id = id,
            Name = name,
            PricePerUnit = price,
            Quantity = quantity,
            CategoryID = categoryId
        };
        string line = $"{product.Id}|{product.Name}|{product.PricePerUnit}|{product.Quantity}|{product.CategoryID}";
        File.AppendAllText(filePath, line + Environment.NewLine);
        Console.WriteLine("Product saved successfully!");
    }

    public void ViewProducts()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No products found");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            Console.WriteLine("No products available");
        }

        Console.WriteLine("ID\tName\tPrice\tQuantity\tCategory");
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 5)
            {
                Console.WriteLine($"{parts[0]}\t{parts[1]}\t{parts[2]}\t{parts[3]}\t\t{parts[4]}");
            }
        }
    }

    public void EditProduct()
    {
        List<string> lines = File.ReadAllLines(filePath).ToList();
        Console.Write("Enter product ID: ");
        long productId = long.Parse(Console.ReadLine());
        for (int i = 0; i < lines.Count; i++) 
        {
            string[] parts = lines[i].Split('|');
            if (long.Parse(parts[0]) == productId)
            {
                string oldName = parts[1];
                Console.Write($"Current Name: {oldName}, enter new name or press Enter to keep: ");
                string newName = Console.ReadLine();
                if (string.IsNullOrEmpty(newName))
                {
                    newName = oldName;
                }

                parts[1] = newName;

                string oldPrice = parts[2];
                Console.Write($"Current Price: {oldPrice}, enter new price or press Enter to keep: ");
                string newPrice = Console.ReadLine();
                if (string.IsNullOrEmpty(newPrice))
                {
                    newPrice = oldPrice;
                }

                parts[2] = newPrice;

                string oldQuantity = parts[3];
                Console.Write($"Current Quantity: {oldQuantity}, enter new quantity or press Enter to keep: ");
                string newQuantity = Console.ReadLine();
                if (string.IsNullOrEmpty(newQuantity))
                {
                    newQuantity = oldQuantity;
                }

                parts[3] = newQuantity;

                string oldCategory = parts[4];
                Console.Write($"Current Category: {oldCategory}, enter new category or press Enter to keep: ");
                string newCategory = Console.ReadLine();
                if (string.IsNullOrEmpty(newCategory))
                {
                    newCategory = oldCategory;
                }

                parts[4] = newCategory;
                
                int index = lines.IndexOf(lines[i]);
                lines[index] = string.Join('|', parts);
                File.WriteAllLines(filePath, lines);
                Console.WriteLine("Product saved");
            }
        }
    }

    public void DeleteProduct()
    {
        List<string> lines = File.ReadAllLines(filePath).ToList();
        Console.Write("Enter product ID: ");
        long productId = long.Parse(Console.ReadLine());
        bool found = false;
        for (int i = 0; i < lines.Count; i++)
        {
            string[] parts = lines[i].Split('|');
            if (long.Parse(parts[0]) == productId)
            {
                found = true;
                lines.RemoveAt(i);
                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"Product '{parts[1]}' is deleted");
                return;
            }
            
        }
        if (!found)
        {
            Console.WriteLine("Product ID not found");
        }
        
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
        var lines = products.Select(p => $"{p.Id}|{p.Name}|{p.PricePerUnit}|{p.Quantity}");
        File.WriteAllLines(filePath,lines);
    }

}