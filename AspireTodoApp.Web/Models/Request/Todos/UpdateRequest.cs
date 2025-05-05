namespace AspireTodoApp.Web.Models.Request.Todos;

public class UpdateRequest
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}