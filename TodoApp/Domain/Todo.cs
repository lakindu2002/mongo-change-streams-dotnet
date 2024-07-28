using MongoDB.Bson;

namespace TodoApp.Domain;

public class Todo
{
    public ObjectId? Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public IEnumerable<TodoItems> SubTasks { get; set; }
}

public class TodoItems
{
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
