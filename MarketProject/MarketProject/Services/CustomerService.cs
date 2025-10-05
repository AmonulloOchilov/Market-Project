namespace MarketProject.Services;

public class CustomerService
{
    public readonly string filePath;
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
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            Console.WriteLine("No customers available");
        }

        Console.WriteLine(
            "{0,-5} {1,-20} {2,-20} {3,-30} {4,-15}",
            "ID", "Name", "Surname", "Email", "Phone Number"
        );

        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length == 5)
            {
                Console.WriteLine(
                    "{0,-5} {1,-20} {2,-20} {3,-30} {4,-15}",
                    parts[0], parts[1], parts[2], parts[3], parts[4]
                );
            }
        }
    }

    public void EditCustomer()
    {
        List<string> lines = File.ReadAllLines(filePath).ToList();
        Console.Write("Enter Customer ID: ");
        long customerId = long.Parse(Console.ReadLine());
        for (int i = 0; i < lines.Count; i++)
        {
            string[] parts = lines[i].Split('|');
            if (long.Parse(parts[0]) == customerId)
            {
                string oldName = parts[1];
                Console.Write($"Current Name: {oldName}, enter new name or press Enter to keep: ");
                string newName = Console.ReadLine();
                if (string.IsNullOrEmpty(newName))
                {
                    newName = oldName;
                }

                parts[1] = newName;

                string oldSurname = parts[2];
                Console.Write($"Current Surname: {oldSurname}, enter new surname or press Enter to keep: ");
                string newSurname = Console.ReadLine();
                if (string.IsNullOrEmpty(newSurname))
                {
                    newSurname = oldSurname;
                }

                parts[2] = newSurname;

                string oldEmail = parts[3];
                Console.Write($"Current Email: {oldEmail}, enter new email or press Enter to keep: ");
                string newEmail = Console.ReadLine();
                if (string.IsNullOrEmpty(newEmail))
                {
                    newEmail = oldEmail;
                }

                parts[3] = newEmail;

                string oldPhoneNumber = parts[4];
                Console.Write($"Current Phone Number: {oldPhoneNumber}, enter new number or press to keep: ");
                string newPhoneNumber = Console.ReadLine();
                if (string.IsNullOrEmpty(newPhoneNumber))
                {
                    newPhoneNumber = oldPhoneNumber;
                }

                parts[4] = newPhoneNumber;
                int index = lines.IndexOf(lines[i]);
                lines[index] = string.Join('|', parts);
                File.WriteAllLines(filePath, lines);
                Console.WriteLine("Customer saved");
            }
            
        }

    }

    public void DeleteCustomer()
    {
        List<string> lines = File.ReadAllLines(filePath).ToList();
        Console.Write("Enter Customer ID: ");
        long customerId = long.Parse(Console.ReadLine());
        bool found = false;
        for (int i = 0; i < lines.Count; i++)
        {
            string[] parts = lines[i].Split('|');
            if (long.Parse(parts[0]) == customerId)
            {
                found = true;
                lines.RemoveAt(i);
                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"Customer {parts[1]} {parts[2]} is deleted");
                return;
            }
        }

        if (!found)
        {
            Console.WriteLine("Customer ID not found");
        }
    }
}