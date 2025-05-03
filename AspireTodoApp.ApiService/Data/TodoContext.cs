using AspireTodoApp.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireTodoApp.ApiService.Data;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}