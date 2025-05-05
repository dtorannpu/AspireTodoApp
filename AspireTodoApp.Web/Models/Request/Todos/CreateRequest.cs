namespace AspireTodoApp.Web.Models.Request.Todos;

public class CreateRequest
{
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}