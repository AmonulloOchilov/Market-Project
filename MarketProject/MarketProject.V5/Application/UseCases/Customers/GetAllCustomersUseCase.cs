using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Customers;

public class GetAllCustomersUseCase
{
    private readonly IRepository<Customer> _repository;

    public GetAllCustomersUseCase(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public List<Customer> Execute()
    {
        return _repository.GetAll();
    }
}