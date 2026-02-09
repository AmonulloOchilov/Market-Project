using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Orders;

public class AddOrderUseCase
{
    private readonly IRepository<Order> _repository;

    public AddOrderUseCase(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public void Execute(Order order)
    {
        List<Order> orders = _repository.GetAll();
        if (orders.Count == 0)
        {
            order.Id = 1;
        }
        else
        {
            order.Id = orders.Max(o => o.Id) + 1;
        }
        order.OrderDate = DateTime.UtcNow;
        orders.Add(order);
        _repository.SaveAll(orders);
    }
}