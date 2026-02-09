using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Customers;

public class UpdateCustomerUseCase
{
    private readonly IRepository<Customer> _repository;

    public UpdateCustomerUseCase(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public bool Execute(Customer updateCustomer)
    {
        List<Customer> customers = _repository.GetAll();
        var customer = customers.FirstOrDefault(c => c.Id == updateCustomer.Id);
        if (customer == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(updateCustomer.Name))
        {
            customer.Name = updateCustomer.Name;
        }

        if (!string.IsNullOrWhiteSpace(updateCustomer.Surname))
        {
            customer.Surname = updateCustomer.Surname;
        }

        if (!string.IsNullOrWhiteSpace(updateCustomer.Email))
        {
            customer.Email = updateCustomer.Email;
        }

        if (!string.IsNullOrWhiteSpace(updateCustomer.PhoneNumber))
        {
            customer.PhoneNumber = updateCustomer.PhoneNumber;
        }

        _repository.SaveAll(customers);
        return true;
    }
}
