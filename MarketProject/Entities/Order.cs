namespace MarketProject.Entities;

public class Order
{
    public long Id { get; set; }
    public long CustomerId { get; set; } // O -> C
    public DateTime OrderDate { get; set; }
    
    public List<OrderItem> OrderItems { get; set; } // O -> OI
    
    public Order() 
    { 
        OrderItems = new List<OrderItem>(); //Like OrderItems are never null
    }
    public decimal TotalAmount //Total cost of the specific order
    {
        get => OrderItems.Sum(item => item.Amount);
    }
    public void AddItem(Product product, int quantity)
    {
        decimal amount = product.PricePerUnit * quantity;
        OrderItems.Add(new OrderItem
        {
            ProductId = product.Id,
            Quantity = quantity,
            Amount = amount
        });
        
        
        if (product.Quantity - quantity > 0)  
        {
            product.Quantity -= quantity;
        }
        else if (product.Quantity - quantity <= 0)
        {
            Console.WriteLine($"The following product has finished: {product.Name}");
        }
    }
}