namespace MarketProject.Entities;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    public DateOnly ExpireDate { get; set; }
    public decimal PricePerUnit { get; set; }
    public long CategoryID { get; set; } //P -> C
    
}