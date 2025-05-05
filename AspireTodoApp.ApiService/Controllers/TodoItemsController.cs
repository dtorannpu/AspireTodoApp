using AspireTodoApp.ApiService.Dtos.Requests;
using AspireTodoApp.ApiService.Dtos.Responses;
using AspireTodoApp.DataAccess.Contexts;
using AspireTodoApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspireTodoApp.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController(TodoContext context) : ControllerBase
{
    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
    {
        return await context.TodoItems.Select(x => ItemToDto(x)).ToListAsync();
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDto(todoItem);
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoUpdateRequestDto todoDto)
    {
        if (id != todoDto.Id)
        {
            return BadRequest();
        }

        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Title = todoDto.Title;
        todoItem.IsComplete = todoDto.IsComplete;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> PostTodoItem(TodoCreateRequestDto todoDto)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDto.IsComplete,
            Title = todoDto.Title
        };

        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDto(todoItem));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        context.TodoItems.Remove(todoItem);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id) => context.TodoItems.Any(e => e.Id == id);

    private static TodoItemDto ItemToDto(TodoItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        IsComplete = item.IsComplete
    };
}