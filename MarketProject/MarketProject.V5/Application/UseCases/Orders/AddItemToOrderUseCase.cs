using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Orders;

public class AddItemToOrderUseCase
{
    private readonly IRepository<Product> _repository;

    public AddItemToOrderUseCase(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public OrderItem Execute(int productId, int quantity)
    {
        var products = _repository.GetAll();
        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        return new OrderItem()
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.PricePerUnit,
            Quantity = quantity
        };
    }
}