namespace MarketProject.Services;

public class CustomerService
{
    private readonly string filePath;
    public CustomerService()
    {
        string dataFolder =
        "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data";
        if (!Directory.Exists(dataFolder))
        {
            Directory.CreateDirectory(dataFolder);
        }

        filePath = Path.Combine(dataFolder, "customers.txt");
    }

    public void AddCustomer(long id, string name, string surname, string email, string phoneNumber)
    {
        string line = $"{id}|{name}|{surname}|{email}|{phoneNumber}";
        File.AppendAllText(filePath, line + Environment.NewLine);
        Console.WriteLine("Customer saved successfully!");
    }

    public void ViewCustomers()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No customers found");
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            Console.WriteLine("No customers available");
        }

        Console.WriteLine($"ID\tName\t\tSurname\t\tEmail\t\t\t\tPhone Number");
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 5)
            {
                Console.WriteLine($"{parts[0]}\t{parts[1]}\t{parts[2]}\t\t{parts[3]}\t\t{parts[4]}");
            }
        }
    }
}