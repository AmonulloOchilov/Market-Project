using System.Text.Json;

namespace MarketProject.V5.Infrastructure.Repositories;

public class JsonRepository<T> : IRepository<T>
{
    private readonly string _filePath;

    public JsonRepository(string filePath)
    {
        _filePath = filePath;
    }

    public List<T> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new();
    }

    public void SaveAll(List<T> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}