using System.ComponentModel.DataAnnotations;

namespace AspireTodoApp.ApiService.Models;

public class TodoItem
{
    public long Id { get; set; }
    [MaxLength(100)] public string? Name { get; set; }
    public bool IsComplete { get; set; }
    [MaxLength(100)] public string? Secret { get; set; }
}