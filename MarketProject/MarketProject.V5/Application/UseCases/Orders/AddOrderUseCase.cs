using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Orders;

public class AddOrderUseCase
{
    private readonly IRepository<Order> _repository;
    private readonly IRepository<Product> _productRepository;

    public AddOrderUseCase(IRepository<Order> repository, IRepository<Product> productRepository)
    {
        _repository = repository;
        _productRepository = productRepository;
    }

    public void Execute(Order order)
    {
        List<Order> orders = _repository.GetAll();
        var products = _productRepository.GetAll();
        if (orders.Count == 0)
        {
            order.Id = 1;
        }
        else
        {
            order.Id = orders.Max(o => o.Id) + 1;
        }
        order.OrderDate = DateTime.UtcNow;
        
        foreach (var item in order.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (product.Quantity < item.Quantity)
            {
                throw new Exception($"Not enough stock for {product.Name}");
            }
            product.Quantity -= item.Quantity;
        }
        _productRepository.SaveAll(products);
        orders.Add(order);
        _repository.SaveAll(orders);
    }
}