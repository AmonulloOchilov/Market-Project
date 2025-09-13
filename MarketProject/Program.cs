// See https://aka.ms/new-console-template for more information

using MarketProject.Entities;

Console.WriteLine("The Market Project starting ...");

Customer customer1 = new Customer();
customer1.Id = 1;
customer1.Name = "Amin";
customer1.Surname = "Ochilov";
customer1.Email = "amonullo436@gmail.com";
customer1.PhoneNumber = "+992987846169";

Product product1 = new Product();
product1.Id = 1;
product1.Name = "Milk";
product1.Quantity = 10;
product1.ExpireDate = new DateTime(2025, 09, 20);
product1.PricePerUnit = 12.5m;
product1.CategoryID = 1; // 1 is Dairy

Product product2 = new Product();
product2.Id = 2;
product2.Name = "Bread";
product2.Quantity = 20;
product2.ExpireDate = new DateTime(2025, 09, 20);
product2.PricePerUnit = 3.0m;
product2.CategoryID = 2; // 2 is Bakery



Order order1 = new Order();
order1.Id = 1;
order1.CustomerId = 1;
order1.OrderDate = new DateTime(2025, 09, 13);
order1.AddItem(product1, 2);
Console.WriteLine($"Order total: {order1.TotalAmount}"); // 25