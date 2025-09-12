namespace MarketProject.Entities;

public class Order
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    
}