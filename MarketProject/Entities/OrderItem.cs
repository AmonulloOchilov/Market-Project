namespace MarketProject.Entities;

public class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; } //OI -> P
    public double Quantity { get; set; }
    public decimal Amount { get; set; }

}