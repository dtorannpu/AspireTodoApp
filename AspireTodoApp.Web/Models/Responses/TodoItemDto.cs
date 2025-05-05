namespace AspireTodoApp.Web.Models.Responses;

public class TodoItemDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
    public string? Description { get; set; }
}