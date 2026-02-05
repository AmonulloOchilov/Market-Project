using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.UseCases.Products;

public class UpdateProductUseCase
{
    private readonly IRepository<Product> _repository;

    public UpdateProductUseCase(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public void Execute(Product updateProduct)
    {
        List<Product> products = _repository.GetAll();
        var product = products.FirstOrDefault(p => p.Id == updateProduct.Id);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        product.Name = updateProduct.Name;
        product.Quantity = updateProduct.Quantity;
        product.ExpireDate = updateProduct.ExpireDate;
        product.PricePerUnit = updateProduct.PricePerUnit;
        product.CategoryId = updateProduct.CategoryId;
        
        _repository.SaveAll(products);
    }
}