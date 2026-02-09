namespace MarketProject.V5.Domain;

public class OrderItem
{ 
    public long ProductId { get; set; } //OI -> P
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Amount => Price * Quantity;
}