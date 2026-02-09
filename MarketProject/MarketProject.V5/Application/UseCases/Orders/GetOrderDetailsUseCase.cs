using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Orders;

public class GetOrderDetailsUseCase
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Customer> _customerRepository;

    public GetOrderDetailsUseCase(IRepository<Order> orderRepository, IRepository<Customer> customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public (Order order, Customer customer) Execute(int orderId)
    {
        var orders = _orderRepository.GetAll();
        var order = orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        var customers = _customerRepository.GetAll();
        var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);
        if (customer == null)
        {
            throw new Exception("Customer not found");
        }
        return (order, customer);
    }
}