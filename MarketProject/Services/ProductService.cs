namespace MarketProject.Services;

public class ProductService
{
    private readonly string filePath;

    public ProductService()
    {
        string dataFolder = "Data";
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
}