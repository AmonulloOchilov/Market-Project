namespace MarketProject.V5.Application.Abstractions;

public interface IRepository<T>
{
    List<T> GetAll();
    void SaveAll(List<T> items);
}