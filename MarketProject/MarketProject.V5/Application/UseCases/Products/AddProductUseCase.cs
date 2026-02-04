using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Products;

public class AddProductUseCase
{
    private readonly IRepository<Product> _repository;

    public AddProductUseCase(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public void Execute(Product product)
    {
        List<Product> products = _repository.GetAll();
        products.Add(product);
        _repository.SaveAll(products);

    }
}