namespace AspireTodoApp.ApiService.Dtos.Requests;

public class TodoCreateRequestDto
{
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}