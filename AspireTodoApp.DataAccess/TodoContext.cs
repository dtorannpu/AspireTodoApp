using AspireTodoApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspireTodoApp.DataAccess;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;
}