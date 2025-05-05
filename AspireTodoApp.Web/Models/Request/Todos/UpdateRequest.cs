namespace AspireTodoApp.Web.Models.Request.Todos;

public class UpdateRequest
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
    public string? Description { get; set; }
}