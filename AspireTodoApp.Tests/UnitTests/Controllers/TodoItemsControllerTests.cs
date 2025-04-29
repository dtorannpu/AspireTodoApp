using AspireTodoApp.ApiService.Controllers;
using AspireTodoApp.ApiService.Dtos;
using AspireTodoApp.ApiService.Models;
using AspireTodoApp.Tests.UnitTests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AspireTodoApp.Tests.UnitTests.Controllers;
public class TodoItemsControllerTests
{
    [Fact]
    public async Task GetTodoItems()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.GetTodoItems();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
        Assert.Empty(items);
    }

    [Fact]
    public async Task GetTodoItems_ReturnsItems_WhenItemsExist()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();
        context.TodoItems.Add(new TodoItem { Id = 1, Name = "Test Item 1", IsComplete = false });
        context.TodoItems.Add(new TodoItem { Id = 2, Name = "Test Item 2", IsComplete = true });
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);

        // Act
        var result = await controller.GetTodoItems();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<TodoItemDTO>>>(result);
        var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDTO>>(okResult.Value);
        Assert.Equal(2, items.Count());
        Assert.Contains(items, i => i.Name == "Test Item 1");
        Assert.Contains(items, i => i.Name == "Test Item 2");
    }
}
