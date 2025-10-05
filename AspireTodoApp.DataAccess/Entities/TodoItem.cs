using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireTodoApp.DataAccess.Entities;

public class TodoItem
{
    public long Id { get; set; }
    [Required][MaxLength(100)] public string? Title { get; set; }
    public bool IsComplete { get; set; }
    [MaxLength(100)] public string? Secret { get; set; }

    [Column(TypeName = "TEXT")]
    [MaxLength(3000)]
    public string? Description { get; set; }
}