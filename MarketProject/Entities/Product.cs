namespace MarketProject.Entities;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    public DateTime ExpireDate { get; set; }
    public decimal PricePerUnit { get; set; }
    public long CategoryID { get; set; } //P -> C
    
}