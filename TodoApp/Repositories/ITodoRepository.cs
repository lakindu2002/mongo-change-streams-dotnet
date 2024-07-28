using TodoApp.Domain;

namespace TodoApp.Repositories;

public interface ITodoRepository<T> where T : Todo
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<T> CreateAsync(T todo);
    Task UpdateAsync(T todo);
    Task DeleteAsync(string id);
}
