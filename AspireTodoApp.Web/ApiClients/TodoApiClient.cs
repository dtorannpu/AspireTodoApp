using AspireTodoApp.Web.Models.Request.Todos;
using AspireTodoApp.Web.Models.Responses;

namespace AspireTodoApp.Web.ApiClients;

public class TodoApiClient(HttpClient httpClient)
{
    public async Task<TodoItemDto[]> GetTodoItemsAsync(CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<TodoItemDto[]>("/api/todoitems", cancellationToken) ?? [];

    public async Task<TodoItemDto?> GetTodoItemAsync(long id, CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<TodoItemDto>($"/api/todoitems/{id}", cancellationToken);

    public async Task<HttpResponseMessage> PostTodoItemAsync(CreateRequest request) =>
        await httpClient.PostAsJsonAsync("/api/todoitems", request);

    public async Task<HttpResponseMessage> PutTodoItemAsync(long id, UpdateRequest request) =>
        await httpClient.PutAsJsonAsync($"/api/todoitems/{id}", request);
}