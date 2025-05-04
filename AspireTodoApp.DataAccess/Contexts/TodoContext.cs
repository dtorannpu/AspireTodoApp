using AspireTodoApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspireTodoApp.DataAccess.Contexts;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}