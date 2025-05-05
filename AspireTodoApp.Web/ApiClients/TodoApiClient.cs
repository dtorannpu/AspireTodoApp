using AspireTodoApp.Web.Models.Forms;
using AspireTodoApp.Web.Models.Responses;

namespace AspireTodoApp.Web.ApiClients;

public class TodoApiClient(HttpClient httpClient)
{
    public async Task<TodoItemDto[]> GetTodoItemsAsync(CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<TodoItemDto[]>("/api/todoitems", cancellationToken) ?? [];

    public async Task<HttpResponseMessage> PostTodoItemAsync(CreateTodoForm form) =>
        await httpClient.PostAsJsonAsync("/api/todoitems", form);

    public async Task<TodoItemDto?> GetTodoItemAsync(int id, CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<TodoItemDto>($"/api/todoitems/{id}", cancellationToken);
}