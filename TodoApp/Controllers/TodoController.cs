using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Domain;
using TodoApp.Services;

namespace WebApi.Controllers;

[Route("api/todo")]
[AllowAnonymous]
public class TodoController : ControllerBase
{
    private readonly ITodoService todoService;
    public TodoController(ITodoService todoService)
    {
        this.todoService = todoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var todos = await todoService.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var todo = await todoService.GetByIdAsync(id);
        return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Todo todo)
    {
        var createdTodo = await todoService.CreateAsync(todo);
        return Ok(createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Todo todo)
    {
        await todoService.UpdateAsync(todo);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await todoService.DeleteAsync(id);
        return Ok();
    }
}