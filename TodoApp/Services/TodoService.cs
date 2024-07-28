using MongoDB.Bson;
using TodoApp.Domain;
using TodoApp.Repositories;

namespace TodoApp.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository<Todo> _todoRepository;
    public TodoService(ITodoRepository<Todo> todoRepository)
    {
        _todoRepository = todoRepository;
    }
    public Task<Todo> CreateAsync(Todo todo)
    {
        todo.Id ??= ObjectId.GenerateNewId();
        return _todoRepository.CreateAsync(todo);
    }

    public Task DeleteAsync(string id)
    {
        return _todoRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        return _todoRepository.GetAllAsync();
    }

    public Task<Todo> GetByIdAsync(string id)
    {
        return _todoRepository.GetByIdAsync(id);
    }

    public Task UpdateAsync(Todo todo)
    {
        return _todoRepository.UpdateAsync(todo);
    }
}
