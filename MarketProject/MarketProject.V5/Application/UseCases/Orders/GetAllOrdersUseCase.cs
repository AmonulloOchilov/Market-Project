using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Orders;

public class GetAllOrdersUseCase
{
    private readonly IRepository<Order> _repository;

    public GetAllOrdersUseCase(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public List<Order> Execute()
    {
        return _repository.GetAll();
    }
}