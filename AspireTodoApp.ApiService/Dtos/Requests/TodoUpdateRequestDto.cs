namespace AspireTodoApp.ApiService.Dtos.Requests;

public class TodoUpdateRequestDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}