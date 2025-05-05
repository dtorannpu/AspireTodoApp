namespace AspireTodoApp.Web.Models.Request.Todos;

public class CreateRequest
{
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}