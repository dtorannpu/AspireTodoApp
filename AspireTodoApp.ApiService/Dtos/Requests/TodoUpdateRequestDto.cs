using System.ComponentModel.DataAnnotations;

namespace AspireTodoApp.ApiService.Dtos.Requests;

public class TodoUpdateRequestDto
{
    [Required] public long Id { get; set; }
    [Required][MaxLength(100)] public string? Title { get; set; }
    public bool IsComplete { get; set; }
    [MaxLength(3000)] public string? Description { get; set; }
}