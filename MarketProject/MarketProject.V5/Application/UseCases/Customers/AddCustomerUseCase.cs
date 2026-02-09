using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Customers;

public class AddCustomerUseCase
{
    private readonly IRepository<Customer> _repository;

    public AddCustomerUseCase(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public void Execute(Customer customer)
    {
        List<Customer> customers = _repository.GetAll();
        if (customers.Count == 0)
        {
            customer.Id = 1;
        }
        else
        {
            customer.Id = customers.Max(c => c.Id) + 1;
        }
        customers.Add(customer);
        _repository.SaveAll(customers);
    }
}