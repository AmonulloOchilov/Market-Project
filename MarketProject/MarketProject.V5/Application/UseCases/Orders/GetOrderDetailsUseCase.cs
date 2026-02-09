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

}