using MarketProject.V5.Application.Abstractions;
using MarketProject.V5.Domain;

namespace MarketProject.V5.Application.Services;

public class ProductService
{
    private IRepository<Product> repository;// This is a Box inside it has GetAll, SaveAll BUT PrService doesnt know what the thing is
    //it could be JsonRepository, DatabaseRepository, FakeRepository

    public ProductService(IRepository<Product> repository)//PrSer does NOT create the repository
    {
        this.repository = repository; //Someone gives it the repository when it is created
        
        //Who is responsible for creating the repository object?
        //The DI container (Program.cs) creates it, cause PrSer should focus only on business logic not object creation!
    }
    
    // public void Add(Product product) //PrSer uses repos-y like a tool
    // {
    //     var products = repository.GetAll();
    //     products.Add(product);
    //     repository.SaveAll(products); //It doesn’t know or care how saving works.
    // }
}