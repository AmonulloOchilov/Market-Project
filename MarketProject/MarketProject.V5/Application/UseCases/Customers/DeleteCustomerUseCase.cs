using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Customers;

public class DeleteCustomerUseCase
{
    private readonly IRepository<Customer> _repository;

    public DeleteCustomerUseCase(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public bool Execute(int customerId)
    {
        var customers = _repository.GetAll();
        var customer = customers.FirstOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            return false;
        }
        customers.Remove(customer);
        _repository.SaveAll(customers);
        return true;
    }
}