namespace AspireTodoApp.ApiService.Dtos.Requests;

public class TodoCreateRequestDto
{
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}