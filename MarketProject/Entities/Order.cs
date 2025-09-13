namespace MarketProject.Entities;

public class Order
{
    public Order()
    {
        OrderItems = new List<OrderItem>();
    }
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public DateTime OrderDate { get; set; }

    public decimal TotalAmount
    {
        get => OrderItems.Sum(item => item.Amount);
    }
    public List<OrderItem> OrderItems { get; set; }

    public void AddItem(Product product, int quantity)
    {
        decimal amount = product.PricePerUnit * quantity;
        OrderItems.Add(new OrderItem
        {
            ProductId = product.Id,
            Quantity = quantity,
            Amount = amount
        });
        product.Quantity = product.Quantity - quantity;
    }
}