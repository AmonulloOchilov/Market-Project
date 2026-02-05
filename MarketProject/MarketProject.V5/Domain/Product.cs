namespace MarketProject.V5.Domain;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Quantity { get; set; }
    public DateOnly ExpireDate { get; set; }
    public decimal PricePerUnit { get; set; }
    public int CategoryId { get; set; }
}