namespace MarketProject.Services;

public class ProductService
{
    private readonly string filePath;

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
        string line = $"{id}|{name}|{price}|{quantity}|{categoryId}";
        File.AppendAllText(filePath, line + Environment.NewLine);
        Console.WriteLine("Product saved successfully!");
    }

    public void ViewProducts()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No products found");
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
}