namespace AspireTodoApp.ApiService.Dtos.Requests;

public class TodoUpdateRequestDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
    public string? Description { get; set; }
}