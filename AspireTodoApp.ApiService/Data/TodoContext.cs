using AspireTodoApp.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireTodoApp.ApiService.Data;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}