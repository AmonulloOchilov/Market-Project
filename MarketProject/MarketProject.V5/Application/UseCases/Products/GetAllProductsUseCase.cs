using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Products;

public class GetAllProductsUseCase
{
    private readonly IRepository<Product> _repository;

    public GetAllProductsUseCase(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public List<Product> Execute()
    {
        return _repository.GetAll();
    }
}