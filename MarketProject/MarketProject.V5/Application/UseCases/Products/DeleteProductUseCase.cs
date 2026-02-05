using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Products;

public class DeleteProductUseCase
{
    private readonly IRepository<Product> _repository;

    public DeleteProductUseCase(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public void Execute(int productId)
    {
        var products = _repository.GetAll();
        var product = products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        products.Remove(product);
        _repository.SaveAll(products);
    }
}