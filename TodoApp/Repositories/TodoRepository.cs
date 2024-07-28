using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApp.Configurations;
using TodoApp.Domain;

namespace TodoApp.Repositories;

public class TodoRepository : ITodoRepository<Todo>

{
    private readonly IMongoCollection<Todo> mongoCollection;
    public TodoRepository(IMongoClient mongoClient, IOptions<Database> dbOptions)
    {
        var database = mongoClient.GetDatabase(dbOptions.Value.DatabaseName);
        mongoCollection = database.GetCollection<Todo>(dbOptions.Value.CollectionName);
    }
    public async Task<Todo> CreateAsync(Todo todo)
    {
        await mongoCollection.InsertOneAsync(todo);
        return todo;
    }

    public async Task DeleteAsync(string id)
    {
        await mongoCollection.DeleteOneAsync(x => x.Id.ToString() == id);
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await mongoCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Todo> GetByIdAsync(string id)
    {
        return await mongoCollection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Todo todo)
    {
        await mongoCollection.ReplaceOneAsync(x => x.Id == todo.Id, todo);
    }
}
