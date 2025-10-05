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
    public decimal Payment { get; set; }

    public decimal Change
    {
        get => Payment - TotalAmount;
    }
}