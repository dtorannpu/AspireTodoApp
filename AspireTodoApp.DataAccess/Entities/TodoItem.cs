using System.ComponentModel.DataAnnotations;

namespace AspireTodoApp.DataAccess.Entities;

public class TodoItem
{
    public long Id { get; set; }
    [MaxLength(100)] public string? Title { get; set; }
    public bool IsComplete { get; set; }
    [MaxLength(100)] public string? Secret { get; set; }
}