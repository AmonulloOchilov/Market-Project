using System.Text.Json;
using MarketProject.Entities;

namespace MarketProject.Services
{
    public class CustomerService
    {
        private readonly string filePath;

        public CustomerService()
        {
            string dataFolder = "/Users/amonulloochilov/Desktop/Market Project/MarketProject/MarketProject/Data";
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            filePath = Path.Combine(dataFolder, "customers.json");
        }
        private List<Customer> LoadCustomers()
        {
            if (!File.Exists(filePath))
            {
                return new List<Customer>();
            }

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Customer>();
            }

            return JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
        }

        private void SaveCustomers(List<Customer> customers)
        {
            string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        public void AddCustomer(long id, string name, string surname, string email, string phoneNumber)
        {
            var customers = LoadCustomers();

            if (customers.Any(c => c.Id == id))
            {
                Console.WriteLine("Customer with this ID already exists!");
                return;
            }

            customers.Add(new Customer
            {
                Id = id,
                Name = name,
                Surname = surname,
                Email = email,
                PhoneNumber = phoneNumber
            });

            SaveCustomers(customers);
            Console.WriteLine("Customer saved");
        }

        public void ViewCustomers()
        {
            var customers = LoadCustomers();
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers available");
                return;
            }

            Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-30} {4,-15}",
                "ID", "Name", "Surname", "Email", "Phone Number");

            foreach (var c in customers)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-20} {3,-30} {4,-15}",
                    c.Id, c.Name, c.Surname, c.Email, c.PhoneNumber);
            }
        }

        public void EditCustomer()
        {
            var customers = LoadCustomers();
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found");
                return;
            }

            Console.Write("Enter Customer ID: ");
            long id = long.Parse(Console.ReadLine()!);

            var customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.Write($"Current Name ({customer.Name}): ");
            string newName = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(newName)) customer.Name = newName;

            Console.Write($"Current Surname ({customer.Surname}): ");
            string newSurname = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(newSurname)) customer.Surname = newSurname;

            Console.Write($"Current Email ({customer.Email}): ");
            string newEmail = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(newEmail)) customer.Email = newEmail;

            Console.Write($"Current Phone Number ({customer.PhoneNumber}): ");
            string newPhone = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(newPhone))
            {
                customer.PhoneNumber = newPhone;
            }

            SaveCustomers(customers);
            Console.WriteLine("Customer updated successfully!");
        }
        public void DeleteCustomer()
        {
            var customers = LoadCustomers();
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found");
                return;
            }

            Console.Write("Enter Customer ID: ");
            long id = long.Parse(Console.ReadLine()!);

            var customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                Console.WriteLine("Customer ID not found");
                return;
            }
            customers.Remove(customer);
            SaveCustomers(customers);
            Console.WriteLine($"Customer {customer.Name} {customer.Surname} is deleted.");
        }
        
    }
    
    
}
