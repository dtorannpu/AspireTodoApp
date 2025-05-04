using AspireTodoApp.Web.Models;

namespace AspireTodoApp.Web.ApiClients;

public class TodoApiClient(HttpClient httpClient)
{
    public async Task<TodoItem[]> GetTodoItemsAsync(CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<TodoItem[]>("/api/todoitems", cancellationToken) ?? [];
}